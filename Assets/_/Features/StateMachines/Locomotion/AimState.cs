using Data.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Runtime
{
    public class AimState : AState, IStateMachine
    {
        #region Methods

        public AimState(StateMachineCore stateMachine, Dictionary<string, object> parameterDictionary = null) : base(stateMachine, parameterDictionary)
        {
            this._stateMachine = stateMachine;
            this._parameterDictionary = parameterDictionary;
        }

        public void Enter()
        {
            _playerRigidbody.velocity = Vector3.zero;
            _shoulderCamera.Priority = 10;
        }

        public void Exit()
        {
            _shoulderCamera.Priority = 0;
        }

        public void Tick()
        {
            if (!_playerBlackboard.GetValue<bool>("IsAiming"))
                _stateMachine.SetState(_explorationState);
        }

        public void FixedTick()
        {

        }

        public void LateTick()
        {

        }

        #endregion
    }
}
