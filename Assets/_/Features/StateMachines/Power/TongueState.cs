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
        }

        public void Enter()
        {
            _tongueDistanceCovered = 0;
            _inputReader.TongueEvent += SendTongue;
        }

        public void Exit()
        {
            _inputReader.TongueEvent -= SendTongue;
        }

        public void Tick()
        {
            _tongueBlackboard.SetValue<bool>("IsTongueReturned", _isTongueReturning);
        }

        public void FixedTick()
        {
            
        }

        public void LateTick()
        {
            // Tongue cooldown
            if (_isTongueExtending) ExtendTongue();
            else if (_isTongueReturning) ReturnTongue();
        }

        #endregion

        #region Utils

        private void SendTongue()
        {
            if (_isTongueExtending || _isTongueReturning) return;

            _tongueTipTransform.parent = null;

            // Check if AimState or ExplorationState
            if (_playerBlackboard.GetValue<bool>("IsAiming"))
                StartRaycastSendingTongue();
            else
                StartLockedSendingTongue();
        }

        private void StartRaycastSendingTongue()
        {
            _isTongueExtending = true;
            _tongueDistanceCovered = 0f;

            RaycastHit hit;
            if (Physics.Raycast(_mainCameraTransform.position, _mainCameraTransform.forward, out hit, _tongueStats.m_maxDistance))
            {
                if (hit.collider.TryGetComponent(out _interactable))
                    HandleInteractableHit(hit.collider.transform);
            }
        }

        private void StartLockedSendingTongue()
        {
            GameObject currentLock = _tongueBlackboard.GetValue<GameObject>("currentLockedTarget");
            if (currentLock != null && Vector3.Distance(_playerBlackboard.GetValue<Vector3>("Position"), currentLock.transform.position) <= _tongueStats.m_maxDistance)
            {
                _tongueHitTransform = currentLock.transform;
                HandleInteractableHit(_tongueHitTransform);
            }
            else
                StartTongueReturn();
        }

        private void ExtendTongue()
        {
            if (_tongueDistanceCovered >= _tongueStats.m_maxDistance)
            {
                StartTongueReturn();
                return;
            }

            float distanceToMove = _tongueStats.m_speed * Time.deltaTime;

            // In ExplorationState, move toward the locked object (if it's there)
            if (_tongueHitTransform != null)
            {
                _tongueTipTransform.position = Vector3.MoveTowards(
                    _tongueTipTransform.position,
                    _tongueHitTransform.position,
                    distanceToMove
                );
            }
            else
                // Otherwise, move the tongue in the player's forward direction
                _tongueTipTransform.position += _tongueTipTransform.forward * distanceToMove;

            _tongueDistanceCovered += distanceToMove;

            // Check if interactable obstacle
            RaycastHit hit;
            if (Physics.SphereCast(_tongueTipTransform.position, .1f, _tongueTipTransform.forward, out hit, distanceToMove))
            {
                if (hit.collider.TryGetComponent(out _interactable))
                    HandleInteractableHit(hit.collider.transform);
            }
        }


        private void HandleInteractableHit(Transform interactableTransform)
        {
            _isTongueExtending = false;

            if (interactableTransform.TryGetComponent<IAmInteractable>(out _interactable))
            {
                if (_interactable.m_grapSize == IAmInteractable.Size.Small)
                {
                    interactableTransform.position = Vector3.MoveTowards(
                        interactableTransform.position,
                        _playerBlackboard.GetValue<Vector3>("Position"),
                        _tongueStats.m_speed * Time.deltaTime
                    );
                }
                else if (_interactable.m_grapSize == IAmInteractable.Size.Large)
                {
                    _playerTransform.position = Vector3.MoveTowards(
                        _playerBlackboard.GetValue<Vector3>("Position"),
                        interactableTransform.position,
                        _tongueStats.m_speed * Time.deltaTime
                    );
                }
            }

            StartTongueReturn();
        }

        private void StartTongueReturn()
        {
            _isTongueExtending = false;
            _isTongueReturning = true;
        }

        private void ReturnTongue()
        {
            _tongueTipTransform.localPosition = Vector3.MoveTowards(_tongueTipTransform.localPosition, _playerBlackboard.GetValue<Vector3>("Position"), _tongueStats.m_speed * Time.deltaTime);
            
            if (Vector3.Distance(_tongueTipTransform.localPosition, _playerBlackboard.GetValue<Vector3>("Position")) < .01f)
            {
                _isTongueReturning = false;
                _tongueTipTransform.localPosition = _playerBlackboard.GetValue<Vector3>("Position");
                _tongueTipTransform.parent = _initialTongueParent;
                _tongueTipTransform.localPosition = Vector3.zero;
            }

            _stateMachine.SetState(_powerlessState);
        }

        #endregion

        #region Privates

        // Interfaces
        private IAmInteractable _interactable;

        // Tongue
        private GameObject _tongueTip;
        private Transform _tongueTipTransform;
        private Vector3 _tongueHitPoint;
        private Transform _tongueHitTransform;
        private Transform _initialTongueParent;

        private bool _isTongueExtending, _isTongueReturning;
        private float _tongueDistanceCovered;

        #endregion
    }
}
