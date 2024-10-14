using Data.Runtime;
using Sirenix.OdinInspector;
using StateMachine.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Runtime
{
    public class PowerBehaviour : MonoBehaviour
    {
        #region Unity

        private void Awake()
        {
            // Components
            TryGetComponent<Transform>(out _transform);

            // Dictionary
            _sharedParameterDictionary.Add("transform", _transform);
            _sharedParameterDictionary.Add("playerBlackboard", _playerBlackboard);
            _sharedParameterDictionary.Add("playerStats", _playerStats);
        }

            private void Start()
        {
            // StateMachine
            _stateMachine = new();
            PowerlessState powerlessState = new(_stateMachine, _inputReader, _sharedParameterDictionary);

            // Initial State
            _stateMachine.SetState(powerlessState);
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedTick();
        }

        private void LateUpdate()
        {
            _stateMachine.LateTick();
        }

        #endregion

        #region Utils

        #endregion

        #region Privates

        // Dictionary
        private Dictionary<string, object> _sharedParameterDictionary = new();

        // StateMachine
        private StateMachineCore _stateMachine;

        // Components
        private Transform _transform;

        // Input
        [Title("ScriptableObjects")]
        [SerializeField]
        private GameInputObject _inputReader;
        // Blackboard
        [SerializeField]
        private Blackboard _playerBlackboard;
        // Base Stats
        [SerializeField]
        private PlayerStats _playerStats;

        #endregion
    }
}
