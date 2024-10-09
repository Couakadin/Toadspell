namespace Player.Runtime
{
    public interface IPlayerStateMachine
    {
        #region Methods

        public void Enter(); // What appends when the enemy enters this state
        public void Tick(); // What appends each frame when the enemy is in this state
        public void Exit(); // What appends when the enemy exists this state

        #endregion
    }
}
