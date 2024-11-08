using Data.Runtime;
using UnityEngine;

namespace Player.Runtime
{
    public class ParryState : IState
    {
        #region Publics

        public StateMachine m_stateMachine { get; }

        #endregion

        #region Methods

        public ParryState(StateMachine stateMachine) 
        {
            m_stateMachine = stateMachine;
            _timer = new Timer(m_stateMachine.m_powerBehaviour.m_durationOfParry);
        }

        public void Enter()
        {
            _timer.OnTimerFinished += ChangeState;
            _timer.Reset();
            _timer.Begin();
            m_stateMachine.m_powerBehaviour.m_parryObject.SetActive(true);
            m_stateMachine.m_powerBehaviour.m_playerAnimator.SetTrigger("Attack");
        }

        public void Exit()
        {
            m_stateMachine.m_powerBehaviour.m_parryObject.SetActive(false);
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

        private Timer _timer;

        #endregion

    }
}
