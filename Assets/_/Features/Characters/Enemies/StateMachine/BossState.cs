using Data.Runtime;
using UnityEngine;

namespace Enemies.Runtime
{
    public class BossState : IState
    {
        #region Publics

        public StateMachine m_stateMachine { get; }

        #endregion

        #region Methods

        public BossState(StateMachine stateMachine)
        {
            m_stateMachine = stateMachine;
            _bossBehaviour = m_stateMachine.m_bossBehaviour;

            m_timer = new(_bossBehaviour.m_timeIdle);
        }

        public void Enter()
        {
            m_timer?.Begin();
            m_timer.OnTimerFinished += ChangeState;
        }

        public void Exit()
        {
            m_timer?.Reset();
            m_timer.OnTimerFinished -= ChangeState;
        }

        public void Tick()
        {
            m_timer?.Tick();
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

        private void ChangeState()
        {
            int rand = Random.Range(0, 3);

            switch(rand)
            {
                case 0: m_stateMachine.ChangeState(m_stateMachine.m_collapseState);
                    break;
                case 1: m_stateMachine.ChangeState(m_stateMachine.m_lineState);
                    break;
                case 2: m_stateMachine.ChangeState(m_stateMachine.m_zoneState);
                    break;
            }
        }

        #endregion

        #region Privates

        private BossBehaviour _bossBehaviour;
        private Timer m_timer;

        #endregion
    }
}
