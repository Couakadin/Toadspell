using UnityEngine;
using Data.Runtime;

namespace Player.Runtime
{
    public class TongueState : IState
    {
        #region Methods

        public StateMachine m_stateMachine { get; }

        public TongueState(StateMachine stateMachine)
        {
            m_stateMachine = stateMachine;

            // Player
            m_stateMachine.m_powerBehaviour.TryGetComponent(out _moveBehaviour);
            m_stateMachine.m_powerBehaviour.TryGetComponent(out _characterController);
            _playerTransform = m_stateMachine.m_powerBehaviour.transform;

            // Tongue
            m_stateMachine.m_powerBehaviour.m_tongue.TryGetComponent(out _tongueRigidbody);
            _tongueSpeed = m_stateMachine.m_powerBehaviour.m_tongueSpeed;

            // Lock
            _detectionLayer = m_stateMachine.m_powerBehaviour.m_detectionLayer;
        }

        public void Enter()
        {
            _tongueMaxDistance = m_stateMachine.m_powerBehaviour.m_detectionRadius;
            _currentLockTarget = m_stateMachine.m_lockState.m_currentLockTarget?.transform;
            if (_currentLockTarget == null) m_stateMachine.ChangeState(m_stateMachine.m_lockState);
        }

        public void Exit()
        {
            // Bools reset
            _isTongueExtended = false;
            _isTongueInteract = false;
            _isTongueReturned = false;
            _isTongueControl = false;
            _isPlayerAttract = false;
        }

        public void Tick()
        {
            if (!_isTongueExtended) DetectTarget();
            if (_hit.collider == null) return;
            if (!_isTongueInteract) TongueExtend();
            if (_isTongueControl) TongueControl();
            if (_isPlayerAttract) PlayerAttract();

            if (_isTongueReturned) TongueReturn();
        }

        public void PhysicsTick()
        {

        }

        public void FinalTick()
        {
            if (_hit.collider == null) return;
            _playerTransform.LookAt(new Vector3(_hit.point.x, _playerTransform.position.y, _hit.point.z));
        }

        public void HandleInput()
        {

        }

        #endregion

        #region Utils

        private void DetectTarget()
        {
            // Normalized direction between player and current lock target
            _directionToTarget = (_currentLockTarget.position - _playerTransform.position).normalized;

            // Raycast to the current lock target
            if (Physics.Raycast(_playerTransform.position, _directionToTarget, out _hit, _tongueMaxDistance, _detectionLayer))
            {
                _hit.collider?.TryGetComponent(out _sizeable);
                _hit.collider?.TryGetComponent(out _fixedJoint);
            }
        }

        private void TongueExtend()
        {
            _isTongueExtended = true;
            _tongueRigidbody.transform.parent = null;
            _tongueRigidbody.gameObject.SetActive(true);

            // Calculate the vector from the tongue to the target point
            _distanceToTarget = _hit.point - _tongueRigidbody.position;

            // Check if the tongue is close enough to stop at the target point using sqrMagnitude
            if (_distanceToTarget.sqrMagnitude > 1f)
                // Move towards the target point by setting the velocity directly to avoid excessive force accumulation
                _tongueRigidbody.velocity = _tongueSpeed * _distanceToTarget.normalized;
            else
            {
                // Stop the tongue when close enough to the target
                _tongueRigidbody.velocity = Vector3.zero;
                TongueInteract();
            }
        }

        private void TongueInteract()
        {
            _isTongueInteract = true;

            if (_sizeable == null) return;

            if (_sizeable.size == ISizeable.Size.small)
            {
                _fixedJoint.connectedBody = _tongueRigidbody;
                _isTongueReturned = true;
            }
            else if (_sizeable.size == ISizeable.Size.medium)
            {
                _fixedJoint.connectedBody = _tongueRigidbody;
                _isTongueControl = true;
            }
            else if (_sizeable.size == ISizeable.Size.large) _isPlayerAttract = true;
        }

        private void TongueControl()
        {
            _tongueMaxDistance = 15f;
            _distanceToTarget = _tongueRigidbody.position - _playerTransform.position;

            if (_distanceToTarget.sqrMagnitude < _tongueMaxDistance * _tongueMaxDistance) return;
            _limitedPosition = _playerTransform.position + _distanceToTarget.normalized * _tongueMaxDistance;
            _tongueRigidbody.MovePosition(_limitedPosition);

            if (m_stateMachine.m_powerBehaviour.m_tongueInput.triggered)
            {
                _fixedJoint.connectedBody = null;
                _isTongueReturned = true;
            }
        }

        private void PlayerAttract()
        {
            _distanceToTarget = _tongueRigidbody.transform.position - new Vector3(_playerTransform.position.x, (_playerTransform.position.y + 3.45f), _playerTransform.position.z);

            if (_distanceToTarget.sqrMagnitude > 2f)
            {
                _moveBehaviour.enabled = false;
                _characterController.Move(Time.deltaTime * _tongueSpeed * _distanceToTarget.normalized);
            }
            else
            {
                _moveBehaviour.enabled = true;
                _isTongueReturned = true;
            }
        }

        private void TongueReturn()
        {
            _distanceToTarget = new Vector3(_playerTransform.position.x, (_playerTransform.position.y + 3.45f), _playerTransform.position.z) - _tongueRigidbody.position;

            if (_distanceToTarget.sqrMagnitude > 3f)
                _tongueRigidbody.velocity = _tongueSpeed * _distanceToTarget.normalized;
            else
            {
                // Params reset
                if (_fixedJoint) _fixedJoint.connectedBody = null;

                _tongueRigidbody.velocity = Vector3.zero;
                _tongueRigidbody.transform.parent = _playerTransform;
                _tongueRigidbody.gameObject.SetActive(false);

                m_stateMachine.ChangeState(m_stateMachine.m_lockState);
            }
        }

        #endregion

        #region Privates

        // Player
        private CharacterController _characterController;
        private MoveBehaviour _moveBehaviour;
        private Transform _playerTransform;
        private float _tongueMaxDistance;
        private bool _isPlayerAttract;

        // Tongue
        private Rigidbody _tongueRigidbody;
        private float _tongueSpeed;
        private bool _isTongueExtended, _isTongueInteract, _isTongueControl, _isTongueReturned;
        private Vector3 _distanceToTarget;
        private Vector3 _limitedPosition;

        // Lock
        private Transform _currentLockTarget;
        private LayerMask _detectionLayer;

        // Physics
        private RaycastHit _hit;
        private Vector3 _directionToTarget;

        // Interact
        private ISizeable _sizeable;
        private FixedJoint _fixedJoint;

        #endregion
    }
}