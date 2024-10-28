using Data.Runtime;

namespace Player.Runtime
{
    public class SpellState : IState
    {
        #region Publics

        public StateMachine m_stateMachine { get; }

        #endregion

        #region Methods

        public SpellState(StateMachine stateMachine) 
        {
            m_stateMachine = stateMachine;
            _timer = new Timer(1f);
        }

        public void Enter()
        {
            _currentPool = m_stateMachine.m_powerBehaviour.m_currentPool;
            _currentPool.GetFirstAvailableObject();

            _timer.Begin();
            _timer.OnTimerFinished += ChangeState;
        }

        public void Exit()
        {
            _timer.Reset();

            _timer.OnTimerFinished -= ChangeState;
        }

        public void Tick()
        {
            _timer.Tick();
        }

        public void PhysicsTick()
        {
            
        }

        public void FinalTick()
        {
            
        }

        public void HandleInput()
        {
            
        }

        #endregion

        #region Utils

        private void ChangeState() => m_stateMachine.ChangeState(m_stateMachine.m_lockState);

        #endregion

        #region Privates

        private PoolSystem _currentPool;
        private Timer _timer;

        #endregion
    }
}
