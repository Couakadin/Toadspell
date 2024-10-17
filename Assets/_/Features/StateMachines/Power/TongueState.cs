using Data.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Runtime
{
    public class TongueState : AState, IAmStateMachine
    {
        #region Methods

        public TongueState(StateMachineCore stateMachine, GameInputObject inputReader, GameObject tongueTip, Dictionary<string, object> parameterDictionary = null) : base(stateMachine, parameterDictionary)
        {
            this._stateMachine = stateMachine;
            this._inputReader = inputReader;
            this._tongueTip = tongueTip;
            this._tongueTipTransform = _tongueTip.transform;
            this._parameterDictionary = parameterDictionary;

            _mainCamera = Camera.main;
            _mainCameraTransform = _mainCamera.transform;

            _initialTongueParent = _tongueTipTransform.parent;

            _tongueCooldownTimer = new Timer(.1f);
            _distanceMaxAim = _tongueStats.m_maxDistanceAim;
            _distanceMaxLock = _tongueStats.m_maxDistanceLock;
        }

        public void Enter()
        {
            _tongueDistanceCovered = 0;
            _tongueTipTransform.parent = null;
            _tongueCooldownTimer?.Reset();
            _goAim = false;
            _goLock = false;
            _tongueCooldownTimer.OnTimerFinished += ReturnPowerless;
        }

        public void Exit()
        {
            _tongueCooldownTimer?.Stop();
            _tongueCooldownTimer.OnTimerFinished -= ReturnPowerless;
        }

        public void Tick()
        {
            _tongueCooldownTimer.Tick();
            _distanceToMove = _tongueStats.m_speed * Time.deltaTime;

            if (_goAim) StartRaycastSendingTongue();
            else if (_goLock) StartLockedSendingTongue();

            if (_goAim || _goLock) return;

            // Step 1: Rotate the player towards the target
            if (_playerBlackboard.GetValue<bool>("IsAiming"))
            {
                if (Physics.Raycast(_mainCameraTransform.position, _mainCameraTransform.forward, out _hit, _tongueStats.m_maxDistanceAim, _playerStats.m_detectionLayer))
                    _goAim = true;
            }
            else if (!_playerBlackboard.GetValue<bool>("IsAiming") && _tongueBlackboard.GetValue<GameObject>("currentLockedTarget") != null)
            {
                _currentLock = _tongueBlackboard.GetValue<GameObject>("currentLockedTarget");
                _goLock = true;
            }
            else ReturnTongue(true);
        }

        public void FixedTick() {  }

        public void LateTick() { }

        #endregion

        #region Utils

        private void RotateToTarget(Transform target)
        {
            // Step 1: Rotate towards the target
            Vector3 direction = (target.position - _playerTransform.position).normalized;

            Quaternion targetRotation = direction != Vector3.zero ? Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)) : _playerTransform.rotation;

            _playerTransform.rotation = Quaternion.Slerp(
                _playerTransform.rotation,
                targetRotation,
                Time.deltaTime * _tongueStats.m_rotationSpeed
            );
        }

        private void StartRaycastSendingTongue()
        {
            RotateToTarget(_hit.transform);
            _tongueDistanceCovered += _distanceToMove;

            // Step 2: Once rotation is complete, move the tongue forward
            _tongueTipTransform.position = Vector3.MoveTowards(
                _tongueTipTransform.position,
                _hit.point,
                _distanceToMove
            );
            
            // Adjust the max distance to the hit point
            _distanceMaxAim = _hit.distance;
            
            // Check if the tongue has reached the target
            if (_tongueDistanceCovered >= _distanceMaxAim)
            {
                // Step 4: If we hit an interactable and the tongue has reached the target, handle the interaction
                if (_hit.collider.TryGetComponent(out _interactable))
                    HandleInteractableHit(_hit.collider.transform);
            }
        }

        private void StartLockedSendingTongue()
        {
            RotateToTarget(_currentLock.transform);

            // Step 2: Once rotation is complete, move the tongue towards the lock
            _tongueTipTransform.position = Vector3.MoveTowards(
                _tongueTipTransform.position,
                _currentLock.transform.position,
                _distanceToMove
            );

            _distanceMaxLock = Vector3.Distance(_tongueBlackboard.GetValue<Vector3>("TonguePosition"), _currentLock.transform.position);

            if (_currentLock.TryGetComponent<Collider>(out Collider targetCollider) && _tongueTipTransform.TryGetComponent<Collider>(out Collider tongueCollider))
            {
                // Distance totale entre la langue et la cible
                _distanceToTarget = Vector3.Distance(_tongueTipTransform.position, _currentLock.transform.position);

                // Calculer la distance de sécurité pour éviter de pénétrer dans la cible
                _stopDistance = tongueCollider.bounds.extents.z + targetCollider.bounds.extents.z;
            }

                // Check if the tongue has reached the locked target
                if (_distanceToTarget <= _stopDistance)
                // Step 4: Handle interaction if the tongue has reached the locked target
                HandleInteractableHit(_currentLock.transform);
        }

        private void HandleInteractableHit(Transform interactableTransform)
        {
            if (interactableTransform.TryGetComponent(out _interactable))
            {
                if (_interactable.m_grapSize == IAmInteractable.Size.Small)
                {
                    // Move the object towards the player but stop at an offset distance in front of the player
                    Vector3 targetPosition = _tongueTipTransform.position;
                    Vector3 directionToPlayer = (interactableTransform.position - targetPosition).normalized;
                    Vector3 offsetPosition = targetPosition - directionToPlayer * _interactable.m_offsetDistance;

                    interactableTransform.position = Vector3.MoveTowards(
                        interactableTransform.position,
                        offsetPosition,
                        _tongueStats.m_speed * Time.deltaTime
                    );
                    ReturnTongue(true);
                }
                else if (_interactable.m_grapSize == IAmInteractable.Size.Large)
                {
                    // Move the player towards the object but stop at an offset distance in front of the object
                    Vector3 objectPosition = interactableTransform.position;
                    Vector3 directionToObject = (objectPosition - _playerBlackboard.GetValue<Vector3>("Position")).normalized;
                    Vector3 offsetPosition = objectPosition - directionToObject * _interactable.m_offsetDistance;

                    _playerTransform.position = Vector3.MoveTowards(
                        _playerBlackboard.GetValue<Vector3>("Position"),
                        offsetPosition,
                        _tongueStats.m_speed * Time.deltaTime
                    );
                    ReturnTongue();
                }
            }
        }

        private void ReturnTongue(bool MustTheTongueReturn = false)
        {
            _initialTonguePosition = _playerBlackboard.GetValue<Vector3>("Position") + new Vector3(0, .65f, 0);

            if (MustTheTongueReturn || Vector3.Distance(_tongueBlackboard.GetValue<Vector3>("TonguePosition"), _initialTonguePosition) <= 1f)
            {
                _tongueTipTransform.position = Vector3.MoveTowards(_tongueBlackboard.GetValue<Vector3>("TonguePosition"), _initialTonguePosition, _tongueStats.m_speed * Time.deltaTime);
            }

            if (Vector3.Distance(_tongueBlackboard.GetValue<Vector3>("TonguePosition"), _initialTonguePosition) <= 1f)
            {
                _tongueTipTransform.position = _initialTonguePosition;
                _tongueTipTransform.parent = _initialTongueParent;
                _goAim = false;
                _goLock = false;
                _tongueCooldownTimer.Begin();
            }
        }

        private void ReturnPowerless() => _stateMachine.SetState(_powerlessState);

        #endregion

        #region Privates

        // Interfaces
        private IAmInteractable _interactable;

        // Tongue
        private GameObject _tongueTip;
        private GameObject _currentLock;

        private Transform _tongueTipTransform;
        private Transform _initialTongueParent;

        private Timer _tongueCooldownTimer;

        private float _tongueDistanceCovered;

        private float _distanceToMove, _distanceMaxAim, _distanceMaxLock;

        private RaycastHit _hit;
        private Vector3 _initialTonguePosition;
        private bool _goAim;
        private bool _goLock;
        private float _distanceToTarget;
        private float _stopDistance;

        #endregion
    }
}
