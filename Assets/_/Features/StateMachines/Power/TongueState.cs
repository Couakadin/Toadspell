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
            // AState Heritage
            _stateMachine = stateMachine;
            _inputReader = inputReader;
            _parameterDictionary = parameterDictionary;


            // Tongue
            _tongueTip = tongueTip;
            _tongueTipTransform = _tongueTip.transform;
            _initialTongueParent = _tongueTipTransform.parent;
            _tongueTip.TryGetComponent(out _tongueRigidbody);
            _tongueTip.TryGetComponent(out _tongueCollider);
            _tongueBounds = _tongueCollider.bounds;

            // Player
            _playerTransform.TryGetComponent(out _playerCollider);
            _playerBounds = _playerCollider.bounds;
            _playerTransform.TryGetComponent(out _playerRigidbody);

            // Timer
            _tongueCooldownTimer = new Timer(.1f);
        }

        public string Name() => "Tongue";

        public void Enter() 
        {
            _tongueRigidbody.position = _playerPosition;
            _tongueDistanceCovered = Vector3.zero;
            _tongueCooldownTimer?.Reset();
            _tongueCooldownTimer.OnTimerFinished += ReturnPowerless;
            _currentLock = _tongueBlackboard.GetValue<GameObject>("currentLockedTarget") ?? null;
            _tongueTip.SetActive(true);
        }

        public void Exit()
        {
            _tongueTip.SetActive(false);
            _tongueCooldownTimer?.Stop();
            _tongueCooldownTimer.OnTimerFinished -= ReturnPowerless;
            TongueStop();
            _goTongueLock = false;
            _goTongueAim = false;
        }

        public void Tick()
        {
            _tongueCooldownTimer.Tick();

            if (!_playerBlackboard.GetValue<bool>("IsAiming") && _currentLock != null)
                _goTongueLock = true;
            else if (_playerBlackboard.GetValue<bool>("IsAiming") && Physics.Raycast(_mainCameraTransform.position, _mainCameraTransform.forward, out _hit, _tongueStats.m_maxDistanceAim/*,_playerStats.m_detectionLayer*/))
                _goTongueAim = true;
        }

        public void FixedTick() 
        {
            if (_goTongueLock && !IsMaxDistanceReached(_tongueStats.m_maxDistanceLock)) StartLockedTongue();
            else if (_goTongueAim && !IsMaxDistanceReached(_hit.distance))
                StartAimTongue();
            else if (IsMaxDistanceReached(_tongueStats.m_maxDistanceLock) || IsMaxDistanceReached(_hit.distance)) TongueReturn();
        }

        public void LateTick() { }

        #endregion

        #region Utils

        private void StartLockedTongue() 
        {
            _targetTransform = _currentLock.transform;
            _playerTransform.LookAt(new Vector3(_targetTransform.position.x, 0f, _targetTransform.position.z));
            TongueMoveTo(_currentLock.transform.position, _currentLock); 
        }

        private void StartAimTongue() 
        {
            _targetTransform = _hit.transform;
            _playerTransform.LookAt(new Vector3(_targetTransform.position.x, 0f, _targetTransform.position.z));
            TongueMoveTo(_hit.point, _hit.collider.gameObject); 
        }

        /// <summary>
        /// Calculate the total distance covered by the tongue.
        /// </summary>
        /// <returns></returns>
        private float TongueDistanceCovered()
        {
            _tongueDistanceCovered += (_tongueTipTransform.forward * Time.fixedDeltaTime) * _tongueStats.m_speed;
            return _tongueDistanceCovered.sqrMagnitude;
        }
        /// <summary>
        /// Compare the current distance covered with a max distance constante.
        /// </summary>
        /// <param name="distanceMax">The max distance to compare.</param>
        /// <returns></returns>
        private bool IsMaxDistanceReached(float distanceMax) => TongueDistanceCovered() >= distanceMax * distanceMax;
        /// <summary>
        /// Stop the tongue at its current emplacement.
        /// </summary>
        private void TongueStop() => _tongueRigidbody.velocity = _tongueRigidbody.angularVelocity = Vector3.zero;
        /// <summary>
        /// Add a force to impulse the tongue.
        /// </summary>
        private void TongueMoveTo(Vector3 targetPosition, GameObject targetCollider)
        {
            _tonguePosition = _tongueBlackboard.GetValue<Vector3>("TonguePosition");
            _directionToTarget = targetPosition - _tonguePosition;

            targetCollider.TryGetComponent(out Collider collider);

            if (collider == null) return;

            if (_directionToTarget.sqrMagnitude >= CombineBounds(_tongueBounds, collider.bounds))
                _tongueRigidbody.velocity = _directionToTarget.normalized * _tongueStats.m_speed;
            else TongueStop();

        }
        private void InteractTongue(Transform target)
        {
            target?.TryGetComponent(out _interactable);
            if (target == null || _interactable == null) return;

            _playerPosition = _playerBlackboard.GetValue<Vector3>("Position");
            _tonguePosition = _tongueBlackboard.GetValue<Vector3>("TonguePosition");

            target.TryGetComponent(out Collider collider);
            target.TryGetComponent(out Rigidbody rigidbody);

            if (rigidbody == null || collider == null) return;

            if (_interactable.m_grapSize == IAmInteractable.Size.Small)
            {
                _directionToTarget = _playerPosition - target.position;

                if (_directionToTarget.sqrMagnitude >= CombineBounds(collider.bounds, _playerBounds, _interactable.m_offsetDistance))
                    rigidbody.MovePosition(target.position + _directionToTarget.normalized * _tongueStats.m_speed * Time.fixedDeltaTime);
            }
            else if (_interactable.m_grapSize == IAmInteractable.Size.Large)
            {
                _directionToTarget = target.position - _playerPosition;

                if (_directionToTarget.sqrMagnitude >= CombineBounds(_playerBounds, collider.bounds, _interactable.m_offsetDistance))
                    _playerRigidbody.velocity = _directionToTarget.normalized * _tongueStats.m_speed;
            }
        }
        /// <summary>
        /// Return the tongue to the player.
        /// </summary>
        private void TongueReturn()
        {
            _playerPosition = _playerBlackboard.GetValue<Vector3>("Position");

            TongueMoveTo(_playerPosition, _playerTransform.gameObject);

            InteractTongue(_targetTransform);

            if ((_tonguePosition - _playerPosition).sqrMagnitude <= CombineBounds(_tongueBounds, _playerBounds, 1f)) _tongueCooldownTimer.Begin();
        }
        /// <summary>
        /// Calculate collider bounds.
        /// </summary>
        /// <param name="boundTarget"></param>
        /// <param name="offsetBound"></param>
        /// <returns></returns>
        private float CombineBounds(Bounds boundStart, Bounds boundTarget, float offsetBound = 0f) => boundStart.extents.magnitude + boundTarget.extents.magnitude + offsetBound;
        /// <summary>
        /// Go back to the Powerless State of the State Machine after the tongue logic.
        /// </summary>
        private void ReturnPowerless() => _stateMachine.SetState(_powerlessState);

        #endregion

        #region Privates

        // Tongue
        private GameObject _tongueTip;
        private Transform _tongueTipTransform;
        private Transform _initialTongueParent;
        private Rigidbody _tongueRigidbody;
        private Vector3 _tonguePosition;
        private Vector3 _directionToTarget;
        private Collider _tongueCollider;
        private Bounds _tongueBounds;

        // Time
        private Timer _tongueCooldownTimer;
        private Vector3 _tongueDistanceCovered;

        // Player
        private Vector3 _playerPosition;
        private Vector3 _directionToPlayer;
        private Collider _playerCollider;
        private Bounds _playerBounds;

        // Target
        private GameObject _currentLock;
        private bool _goTongueLock;
        private bool _goTongueAim;
        private RaycastHit _hit;
        private Transform _targetTransform;
        private IAmInteractable _interactable;

        #endregion
    }
}