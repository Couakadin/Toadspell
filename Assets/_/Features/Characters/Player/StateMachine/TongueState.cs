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
            _tongueMesh = m_stateMachine.m_powerBehaviour.m_tongueMesh;
            _tongueMeshPosition = m_stateMachine.m_powerBehaviour.m_tongueMeshPosition;

            // Lock
            _detectionLayer = m_stateMachine.m_powerBehaviour.m_detectionLayer;

            // Timer
            _timerReturn = new Timer(1.5f);
        }

        public void Enter()
        {
            _timerReturn.OnTimerFinished += TongueReturn;
            _timerReturn?.Reset();

            _tongueMesh.gameObject.SetActive(true);

            _tongueMaxDistance = m_stateMachine.m_powerBehaviour.m_maxDetectionRadius;
            _currentLockTarget = m_stateMachine.m_lockState.m_currentLockTarget?.transform;
            if (_currentLockTarget == null) m_stateMachine.ChangeState(m_stateMachine.m_lockState);
        }

        public void Exit()
        {
            _timerReturn.OnTimerFinished -= TongueReturn;

            _tongueMesh.gameObject.SetActive(false);

            // Bools reset
            _isTongueExtended = false;
            _isTongueInteract = false;
            _isTongueReturned = false;
            _isTongueControl = false;
            _isTongueAttract = false;
            _isTonguePlateform = false;
        }

        public void Tick()
        {
            _timerReturn?.Tick();

            if (_hit.collider != null) _playerTransform.LookAt(new Vector3(_hit.collider.gameObject.transform.position.x, _playerTransform.position.y, _hit.collider.gameObject.transform.position.z));
            
            _tongueMesh.transform.position = _tongueMeshPosition.position;

            if (_isTongueReturned)
            {
                TongueReturn();
                return;
            }

            if (!_isTongueExtended) DetectTarget();
            if (_hit.collider == null) return;
            if (!_isTongueInteract) TongueExtend();
            if (_isTongueControl) TongueControl();
            if (_isTongueAttract) TongueAttract();
        }

        public void PhysicsTick() { }

        public void FinalTick() { }

        public void HandleInput() { }

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
            _tongueMesh.transform.parent = null;

            // Calculate the vector from the tongue to the target point
            _distanceToTarget = _hit.collider.transform.position - _tongueRigidbody.position;

            // Check if the tongue is close enough to stop at the target point using sqrMagnitude
            if (_distanceToTarget.sqrMagnitude > 1f)
            {
                // Move towards the target point by setting the velocity directly to avoid excessive force accumulation
                _tongueRigidbody.velocity = _tongueSpeed * _distanceToTarget.normalized;

                _tongueMesh.LookAt(_hit.collider.transform);
                TongueMesh(Vector3.Distance(_hit.collider.transform.position, _tongueMesh.position));
            }
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

            if (_sizeable == null || _sizeable.size == ISizeable.Size.none)
            {
                _isTongueReturned = true;
                return;
            }

            if (_sizeable.size == ISizeable.Size.small)
            {
                m_stateMachine.m_powerBehaviour.m_canEat = true;
                _fixedJoint.connectedBody = _tongueRigidbody;
                _isTongueReturned = true;
            }
            else if (_sizeable.size == ISizeable.Size.medium)
            {
                _fixedJoint.connectedBody = _tongueRigidbody;
                _isTongueControl = true;
            }
            else if (_sizeable.size == ISizeable.Size.large || _sizeable.size == ISizeable.Size.platform)
            {
                _timerReturn.Begin();

                if (_sizeable.size == ISizeable.Size.platform) _isTonguePlateform = true;
                _isTongueAttract = true;
            }
        }

        private void TongueControl()
        {
            _tongueMaxDistance = 15f;
            _distanceToTarget = _tongueRigidbody.position - _playerTransform.position;

            _tongueMesh.LookAt(_hit.collider.transform);
            TongueMesh(Vector3.Distance(_tongueRigidbody.position, _tongueMesh.position));

            if (_distanceToTarget.sqrMagnitude > _tongueMaxDistance * _tongueMaxDistance)
            {
                _limitedPosition = _playerTransform.position + _distanceToTarget.normalized * _tongueMaxDistance;
                _tongueRigidbody.MovePosition(_limitedPosition);
            }

            if (m_stateMachine.m_powerBehaviour.m_tongueInput.triggered)
            {
                if (_fixedJoint) _fixedJoint.connectedBody = null;
                _isTongueReturned = true;
            }
        }

        private void TongueAttract()
        {
            _distanceToTarget = _tongueRigidbody.transform.position - new Vector3(_playerTransform.position.x, (_playerTransform.position.y + 3.45f), _playerTransform.position.z);

            if (_distanceToTarget.sqrMagnitude > 5f)
            {
                _moveBehaviour.enabled = false;
                _characterController.Move(Time.deltaTime * _tongueSpeed * _distanceToTarget.normalized);

                _tongueMesh.LookAt(_hit.collider.transform);
                TongueMesh(Vector3.Distance(_tongueRigidbody.position, _tongueMesh.position));
            }
            else
            {
                _moveBehaviour.enabled = true;

                if (_isTonguePlateform) _moveBehaviour.m_tonguePlateform = true;
                _moveBehaviour.m_velocity.y = Mathf.Sqrt(_moveBehaviour.m_jump * -3f * _moveBehaviour.m_gravity);

                _isTongueReturned = true;
            }
        }

        private void TongueReturn()
        {
            _distanceToTarget = new Vector3(_playerTransform.position.x, (_playerTransform.position.y + 3.45f), _playerTransform.position.z) - _tongueRigidbody.position;

            if (_distanceToTarget.sqrMagnitude > 1f)
            {
                _tongueMesh.LookAt(_hit.collider.transform);
                TongueMesh(Vector3.Distance(_tongueRigidbody.position, _tongueMesh.position));

                _tongueRigidbody.velocity = _tongueSpeed * _distanceToTarget.normalized;
            }
            else
            {
                // Params reset
                if (_fixedJoint) _fixedJoint.connectedBody = null;
                _tongueRigidbody.velocity = Vector3.zero;
                _tongueRigidbody.transform.parent = _playerTransform;
                _tongueMesh.transform.parent = _playerTransform;
                _tongueMesh.localScale = new Vector3(_tongueMesh.localScale.x, _tongueMesh.localScale.y, .01f);

                m_stateMachine.ChangeState(m_stateMachine.m_lockState);
            }
        }

        private void TongueMesh(float totalDistance)
        {
            // Calculer la distance actuelle entre le rigidbody de la langue et le mesh
            float currentDistance = Vector3.Distance(_tongueRigidbody.position, _tongueMesh.position);

            // Calculer la vitesse d'interpolation en fonction de la distance et du temps
            float interpolationSpeed = _tongueSpeed * Time.deltaTime;

            // Interpoler la taille du mesh de la langue
            _tongueMesh.localScale = Vector3.Lerp(
                _tongueMesh.localScale,
                new Vector3(
                    _tongueMesh.localScale.x,
                    _tongueMesh.localScale.y,
                    currentDistance * 0.035f
                ),
                interpolationSpeed
            );
        }

        #endregion

        #region Privates

        // Player
        private CharacterController _characterController;
        private MoveBehaviour _moveBehaviour;
        private Transform _playerTransform;
        private float _tongueMaxDistance;
        private bool _isTongueAttract, _isTonguePlateform;

        // Tongue
        private Rigidbody _tongueRigidbody;
        private float _tongueSpeed;
        private bool _isTongueExtended, _isTongueInteract, _isTongueControl, _isTongueReturned;
        private Vector3 _distanceToTarget;
        private Vector3 _limitedPosition;
        private Transform _tongueMesh;
        public Transform _tongueMeshPosition;

        // Lock
        private Transform _currentLockTarget;
        private LayerMask _detectionLayer;

        // Physics
        private RaycastHit _hit;
        private Vector3 _directionToTarget;

        // Interact
        private ISizeable _sizeable;
        private FixedJoint _fixedJoint;

        // Timer
        private bool _isFirstPress = false;
        private float _doublePressTimer = 0f;
        private float _doublePressThreshold = 0.3f;
        private Timer _timerReturn;

        #endregion
    }
}