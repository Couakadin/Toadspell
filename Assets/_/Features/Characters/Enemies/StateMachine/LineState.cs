using Data.Runtime;

namespace Enemies.Runtime
{
    public class LineState : IState
    {
        #region Publics

        public StateMachine m_stateMachine { get; }

        #endregion

        #region Methods

        public LineState(StateMachine stateMachine)
        {
            m_stateMachine = stateMachine;
        }

        public void Enter()
        {

        }

        public void Exit()
        {

        }

        public void Tick()
        {

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

        #endregion

        #region Privates



        #endregion
    }
}
