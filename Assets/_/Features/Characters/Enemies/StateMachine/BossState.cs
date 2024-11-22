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

        public void Tick() => m_timer?.Tick();

        public void PhysicsTick() { }

        public void FinalTick() { }

        public void HandleInput() { }

        #endregion

        #region Utils

        /// <summary>
        /// Changes the boss's attack state.
        /// Ensures an attack can be repeated twice in a row but not three times.
        /// </summary>
        private void ChangeState()
        {
            int rand;

            // If the current attack was repeated twice, force a different attack
            if (_repeatCount >= 2)
            {
                do rand = Random.Range(0, 3); // Number of possible attacks
                while (rand == _currentAttack); // Ensure it's not the same as the current attack
            }
            else rand = Random.Range(0, 3); // Select a random attack

            // Update the current attack and its repetition count
            if (rand == _currentAttack) _repeatCount++;
            else
            {
                _currentAttack = rand;
                _repeatCount = 1; // Reset the count for the new attack
            }

            // Execute the corresponding attack state change
            switch (rand)
            {
                case 0:
                    m_stateMachine.ChangeState(m_stateMachine.m_collapseState);
                    break;
                case 1:
                    m_stateMachine.ChangeState(m_stateMachine.m_lineState);
                    break;
                case 2:
                    m_stateMachine.ChangeState(m_stateMachine.m_zoneState);
                    break;
            }
        }

        #endregion

        #region Privates

        private BossBehaviour _bossBehaviour;
        private Timer m_timer;

        // Tracks the current attack type (-1 indicates no initial attack)
        private int _currentAttack = -1;
        // Counts consecutive repetitions of the current attack
        private int _repeatCount = 0;

        #endregion
    }
}
