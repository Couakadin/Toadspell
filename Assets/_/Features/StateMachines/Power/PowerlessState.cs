using Data.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Runtime
{
    public class PowerlessState : AState, IAmStateMachine
    {
        #region Methods

        public PowerlessState(StateMachineCore stateMachine, GameInputObject inputReader, Dictionary<string, object> parameterDictionary = null) : base(stateMachine, parameterDictionary)
        {
            this._stateMachine = stateMachine;
            this._inputReader = inputReader;
            this._parameterDictionary = parameterDictionary;
        }

        public string Name() => "Powerless";

        public void Enter()
        {
            _inputReader.LockEvent += SwitchTarget;
        }

        public void Exit()
        {
            _inputReader.LockEvent -= SwitchTarget;
        }

        public void Tick()
        {
            if (_inputReader.IsPerformed("Tongue"))
                _stateMachine.SetState(_tonguetState);

            if (_inputReader.IsPerformed("Spell"))
                _stateMachine.SetState(_spellState);

            if (!_playerBlackboard.GetValue<bool>("IsAiming"))
            {
                // Update lockable targets with OverlapSphere
                UpdateLockableList();
                _tongueBlackboard.SetValue("currentLockedTarget", _currentLockedTarget);
            }
            else
            {
                UnlockTarget();
                _lockingList.Clear();
            }
        }

        public void FixedTick()
        {

        }

        public void LateTick()
        {

        }

        #endregion

        #region Utils

        /// <summary>
        /// Update the list of lockable objects within the detection radius using OverlapSphere.
        /// </summary>
        private void UpdateLockableList()
        {
            // Get all colliders within the detection radius
            Collider[] hitColliders = Physics.OverlapSphere(_playerTransform.position, _playerStats.m_detectionRadius, _playerStats.m_detectionLayer);

            // Clear the existing list
            _lockingList.Clear();

            // Loop through detected colliders
            foreach (Collider hitCollider in hitColliders)
            {
                // Check if the object has the IAmLockable component
                if (hitCollider.TryGetComponent<IAmLockable>(out IAmLockable lockable))
                    // Add the gameObject of the MonoBehaviour that implements IAmLockable
                    _lockingList.Add(hitCollider.gameObject);
            }

            // If no targets are found, unlock the current locked target
            if (_lockingList.Count == 0) UnlockTarget();
            // If there are targets, ensure the first one is locked if none is currently locked
            else if (_currentLockedTarget == null)
            {
                LockTarget(_lockingList[0]);
                _currentTargetIndex = 0;
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
            target.TryGetComponent<IAmLockable>(out IAmLockable lockable);
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
            _currentLockedTarget.TryGetComponent<IAmLockable>(out IAmLockable lockable);
            if (lockable == null) return;
            lockable.OnUnlock(); // Call the unlock behavior (e.g., revert material to default)

            // Clear the current locked target
            _currentLockedTarget = null;
        }

        #endregion

        #region Privates

        // Lists
        private List<GameObject> _lockingList = new(); // List of detected lockable objects

        private GameObject _currentLockedTarget = null;
        private int _currentTargetIndex = -1; // Index of the currently locked target in the list

        #endregion
    }
}
