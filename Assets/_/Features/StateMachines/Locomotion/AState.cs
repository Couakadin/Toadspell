using Cinemachine;
using Data.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Runtime
{
    public abstract class AState
    {
        #region Methods

        // Setters for state references
        public void SetJumpState(JumpState jumpState) => _jumpState = jumpState;
        public void SetAimState(AimState aimState) => _aimState = aimState;
        public void SetExplorationState(ExplorationState explorationState) => _explorationState = explorationState;

        #endregion

        #region Utils

        protected AState(StateMachineCore stateMachine, Dictionary<string, object> parameterDictionary = null) 
        {
            this._parameterDictionary = parameterDictionary;
            this._playerRigidbody = GetItemFromParameterDictionary<Rigidbody>("rigidbody");
            this._playerTransform = GetItemFromParameterDictionary<Transform>("transform");
            this._playerBlackboard = GetItemFromParameterDictionary<Blackboard>("playerBlackboard");
            this._playerStats = GetItemFromParameterDictionary<PlayerStatsData>("playerStats");
            this._thirdPersonCamera = GetItemFromParameterDictionary<CinemachineFreeLook>("thirdPersonCamera");
            this._shoulderCamera = GetItemFromParameterDictionary<CinemachineVirtualCamera>("shoulderCamera");
            this._mainCamera = Camera.main;
        }

        // Tools
        /// <summary>
        /// Check if a value exists in the parameterDictionary in the State constructor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual T GetItemFromParameterDictionary<T>(string key) where T : class
        {
            if (_parameterDictionary != null && _parameterDictionary.TryGetValue(key, out var value))
                return value as T; // Use of "as" to return the typed object or null
            return null; // Return null if the key does not exist or the type does not match
        }

        #endregion

        #region Privates

        // StateMachine
        protected StateMachineCore _stateMachine;
        // States
        protected ExplorationState _explorationState;
        protected JumpState _jumpState;
        protected AimState _aimState;

        // Dictionary
        protected Dictionary<string, object> _parameterDictionary;

        // Blackboard
        protected Blackboard _playerBlackboard;
        // Stats
        protected PlayerStatsData _playerStats;
        // Input
        protected GameInputObject _inputReader;
        // Components
        /// Physics
        protected Rigidbody _playerRigidbody;
        protected Transform _playerTransform;

        // Cameras
        protected Camera _mainCamera;
        protected CinemachineFreeLook _thirdPersonCamera;
        protected CinemachineVirtualCamera _shoulderCamera;

        #endregion
    }
}
