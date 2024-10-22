using Data.Runtime;
using System.Collections.Generic;

namespace StateMachine.Runtime
{
    public class SpellState : AState, IAmStateMachine
    {
        #region Methods
        public SpellState(StateMachineCore stateMachine, Dictionary<string, object> parameterDictionary = null) : base(stateMachine, parameterDictionary)
        {
            this._stateMachine = stateMachine;
            this._parameterDictionary = parameterDictionary;

            _timer = new Timer(1f);
        }

        public string Name() => "Spell";


        public void Enter()
        {
            _currentPool = _playerBlackboard.GetValue<PoolSystem>("CurrentSpell");

            _timer.Begin();
            
            _timer.OnTimerFinished += SpellTrigger;
            _timer.OnTimerFinished += ChangeState;
        }

        public void Exit()
        {
            _timer.Reset();

            _timer.OnTimerFinished -= SpellTrigger;
            _timer.OnTimerFinished -= ChangeState;
        }

        public void Tick()
        {
            _timer.Tick();
        }

        public void FixedTick()
        {
            
        }

        public void LateTick()
        {
            
        }

        #endregion

        #region Utils

        private void SpellTrigger() => _currentPool.GetFirstAvailableObject();

        private void ChangeState() => _stateMachine.SetState(_powerlessState);

        #endregion

        #region Private

        private PoolSystem _currentPool;
        private Timer _timer;

        #endregion
    }
}
