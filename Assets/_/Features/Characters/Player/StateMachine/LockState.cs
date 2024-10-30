using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Data.Runtime;

namespace Player.Runtime
{
    public class LockState : IState
    {
        #region Methods

        public StateMachine m_stateMachine { get; }

        public GameObject m_currentLockTarget { get; private set; }

        public LockState(StateMachine stateMachine)
        {
            m_stateMachine = stateMachine;

            // Blackboards
            _tongueBlackboard = m_stateMachine.m_powerBehaviour.m_tongueBlackboard;

            // Player
            _playerTransform = m_stateMachine.m_powerBehaviour.transform;

            // Input
            _lockInput = m_stateMachine.m_powerBehaviour.m_lockInput;
            _tongueInput = m_stateMachine.m_powerBehaviour.m_tongueInput;
            _spellInput = m_stateMachine.m_powerBehaviour.m_spellInput;

            // Lock
            _detectionRadius = m_stateMachine.m_powerBehaviour.m_detectionRadius;
            _detectionLayer = m_stateMachine.m_powerBehaviour.m_detectionLayer;

            // Camera
            _cameraMain = Camera.main;
            _cameraTransform = _cameraMain.transform;
        }

        public void Enter()
        {

        }

        public void Exit()
        {

        }

        public void Tick()
        {
            UpdateLockableList();
            m_currentLockTarget = _currentLockedTarget;
        }

        public void PhysicsTick()
        {

        }

        public void FinalTick()
        {
            _tongueBlackboard.SetValue("currentLockedTarget", m_currentLockTarget);
        }

        public void HandleInput()
        {
            if (_lockInput.triggered) SwitchTarget();
            if (_tongueInput.triggered) m_stateMachine.ChangeState(m_stateMachine.m_tongueState);
            if (_spellInput.triggered) m_stateMachine.ChangeState(m_stateMachine.m_spellState);
        }

        #endregion

        #region Utils

        /// <summary>
        /// Updates the list of lockable objects within the detection radius and a 45-degree cone in front of the player.
        /// If the currently locked target exits the detection radius, it will automatically lock onto a new target in the list.
        /// </summary>
        private void UpdateLockableList()
        {
            // Clear the existing list of lockable targets
            _lockingList.Clear();

            // Get all colliders within the detection radius
            _hitColliders = Physics.OverlapSphere(_playerTransform.position, _detectionRadius, _detectionLayer);

            // Loop through the detected colliders
            foreach (Collider hitCollider in _hitColliders)
            {
                // Check if the object is visible on the camera screen
                _viewportPoint = _cameraMain.WorldToViewportPoint(hitCollider.transform.position);

                // Check if the point is within the camera's view (inside screen bounds)
                _isInView = _viewportPoint.z > 0 &&
                            _viewportPoint.x > 0 && _viewportPoint.x < 1 &&
                            _viewportPoint.y > 0 && _viewportPoint.y < 1;

                // If the object is visible on screen and implements ILockable, add it to the list
                if (_isInView && hitCollider.TryGetComponent(out IAmLockable lockable))
                {
                    _lockingList.Add(hitCollider.gameObject);
                }
            }

            // If the current locked target is outside the detection radius or not in the list, auto-lock the first available target
            if (_currentLockedTarget != null && !_lockingList.Contains(_currentLockedTarget))
            {
                UnlockTarget();

                // If there are targets available, lock onto the first one in the list
                if (_lockingList.Count > 0)
                {
                    LockTarget(_lockingList[0]);
                    _currentTargetIndex = 0;
                }
            }
            else if (_lockingList.Count > 0 && _currentLockedTarget == null)
            {
                // If no target is currently locked, lock the first available target
                LockTarget(_lockingList[0]);
                _currentTargetIndex = 0;
            }
            else if (_lockingList.Count == 0)
            {
                // If no targets are found, unlock the currently locked target
                UnlockTarget();
            }
        }

        /// <summary>
        /// Switch to the next target in the locking list when Tab is pressed.
        /// </summary>
        private void SwitchTarget()
        {
            if (_lockingList.Count <= 1) return; // If only one or no target is in the list, don't switch

            // Increment the target index and loop back to the start if necessary
            _currentTargetIndex = (_currentTargetIndex + 1) % _lockingList.Count;

            // Lock the new target
            LockTarget(_lockingList[_currentTargetIndex]);
        }

        /// <summary>
        /// Lock the specified target and change its material to indicate it's locked.
        /// </summary>
        /// <param name="target"></param>
        private void LockTarget(GameObject target)
        {
            // If the target is already locked, do nothing
            if (_currentLockedTarget == target) return;

            // Unlock the previous target
            UnlockTarget();

            // Set the new target and lock it
            _currentLockedTarget = target;
            target.TryGetComponent(out IAmLockable lockable);
            if (lockable == null) return;
            lockable.OnLock(); // Call the lock behavior (e.g., change material to red)
        }

        /// <summary>
        /// Unlock the currently locked target.
        /// </summary>
        private void UnlockTarget()
        {
            // If there's no currently locked target, do nothing
            if (_currentLockedTarget == null) return;

            // Call the OnUnlock method of the currently locked target
            _currentLockedTarget.TryGetComponent(out IAmLockable lockable);
            if (lockable == null) return;
            lockable.OnUnlock(); // Call the unlock behavior (e.g., revert material to default)

            // Clear the current locked target
            _currentLockedTarget = null;
        }

        #endregion

        #region Privates

        // Blackboards
        private Blackboard _tongueBlackboard;

        // Lists
        private List<GameObject> _lockingList = new(); // List of detected lockable objects

        private GameObject _currentLockedTarget = null;
        private int _currentTargetIndex = -1; // Index of the currently locked target in the list

        // Player
        private Transform _playerTransform;
        private Collider[] _hitColliders;

        // Input
        private InputAction _lockInput;
        private InputAction _tongueInput;
        private InputAction _spellInput;

        // Lock
        private float _detectionRadius;
        private LayerMask _detectionLayer;

        // Camera
        private Camera _cameraMain;
        private Transform _cameraTransform;
        private bool _isInView;
        private Vector3 _viewportPoint;

        #endregion
    }
}