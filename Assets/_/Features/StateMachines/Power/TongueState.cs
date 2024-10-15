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

            // Step 1: Rotate the player towards the target
            if (_playerBlackboard.GetValue<bool>("IsAiming"))
            {
                if (Physics.Raycast(_mainCameraTransform.position, _mainCameraTransform.forward, out _hit, _tongueStats.m_maxDistanceAim))
                {
                    RotateToTarget(_hit.transform);

                    // Step 2: Start sending the tongue after rotation is finished
                    _distanceToMove = _tongueStats.m_speed * Time.deltaTime;
                    _tongueDistanceCovered += _distanceToMove;

                    StartRaycastSendingTongue();
                }
            }
            else if (!_playerBlackboard.GetValue<bool>("IsAiming") && _tongueBlackboard.GetValue<GameObject>("currentLockedTarget") != null)
            {
                _currentLock = _tongueBlackboard.GetValue<GameObject>("currentLockedTarget");
                RotateToTarget(_currentLock.transform);

                // Step 2: Start sending the tongue after rotation is finished
                _distanceToMove = _tongueStats.m_speed * Time.deltaTime;
                _tongueDistanceCovered += _distanceToMove;

                StartLockedSendingTongue();
            }
            else ReturnTongue();
        }

        public void FixedTick()
        {
            
        }

        public void LateTick()
        {
            
        }

        #endregion

        #region Utils

        private void RotateToTarget(Transform target)
        {
            // Step 1: Rotate towards the target
            Vector3 direction = (target.position - _playerTransform.position).normalized;

            Quaternion targetRotation = direction != Vector3.zero ? Quaternion.LookRotation(direction) : _playerTransform.rotation;

            _playerTransform.rotation = Quaternion.Slerp(
                _playerTransform.rotation,
                targetRotation,
                Time.deltaTime * _tongueStats.m_rotationSpeed
            );
        }

        private void StartRaycastSendingTongue()
        {
            // Step 2: Once rotation is complete, move the tongue forward
            _tongueTipTransform.position = Vector3.MoveTowards(
                _tongueTipTransform.position,
                _hit.transform.position,
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
                ReturnTongue();
            }
        }

        private void StartLockedSendingTongue()
        {
            // Step 2: Once rotation is complete, move the tongue towards the lock
            _tongueTipTransform.position = Vector3.MoveTowards(
                _tongueTipTransform.position,
                _currentLock.transform.position,
                _distanceToMove
            );

            _distanceMaxLock = Vector3.Distance(_currentLock.transform.position, _tongueTipTransform.position);

            // Check if the tongue has reached the locked target
            if (_tongueDistanceCovered >= _distanceMaxLock)
            {
                // Step 4: Handle interaction if the tongue has reached the locked target
                HandleInteractableHit(_currentLock.transform);
                ReturnTongue();
            }
        }


        private void HandleInteractableHit(Transform interactableTransform)
        {
            if (interactableTransform.TryGetComponent(out _interactable))
            {
                if (_interactable.m_grapSize == IAmInteractable.Size.Small)
                {
                    // Move the object towards the player but stop at an offset distance in front of the player
                    Vector3 targetPosition = _playerBlackboard.GetValue<Vector3>("Position");
                    Vector3 directionToPlayer = (targetPosition - interactableTransform.position).normalized;
                    Vector3 offsetPosition = targetPosition - directionToPlayer * _tongueStats.m_offsetDistance;

                    interactableTransform.position = Vector3.MoveTowards(
                        interactableTransform.position,
                        offsetPosition,
                        _tongueStats.m_speed * Time.deltaTime
                    );
                }
                else if (_interactable.m_grapSize == IAmInteractable.Size.Large)
                {
                    // Move the player towards the object but stop at an offset distance in front of the object
                    Vector3 objectPosition = interactableTransform.position;
                    Vector3 directionToObject = (objectPosition - _playerBlackboard.GetValue<Vector3>("Position")).normalized;
                    Vector3 offsetPosition = objectPosition - directionToObject * _tongueStats.m_offsetDistance;

                    _playerTransform.position = Vector3.MoveTowards(
                        _playerBlackboard.GetValue<Vector3>("Position"),
                        offsetPosition,
                        _tongueStats.m_speed * Time.deltaTime
                    );
                }
            }
        }

        private void ReturnTongue()
        {
            _tongueTipTransform.position = Vector3.MoveTowards(_tongueBlackboard.GetValue<Vector3>("TonguePosition"), _playerBlackboard.GetValue<Vector3>("Position"), _tongueStats.m_speed * Time.deltaTime);
            
            if (Vector3.Distance(_tongueBlackboard.GetValue<Vector3>("TonguePosition"), _playerBlackboard.GetValue<Vector3>("Position")) < .1f)
            {
                _tongueTipTransform.position = _playerBlackboard.GetValue<Vector3>("Position");
                _tongueTipTransform.parent = _initialTongueParent;
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

        #endregion
    }
}
