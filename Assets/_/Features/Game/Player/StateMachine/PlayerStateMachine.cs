namespace Player.Runtime
{
    public class PlayerStateMachine
    {
        #region Methods

        public void SetState(IPlayerStateMachine newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Tick() => _currentState?.Tick();

        #endregion

        #region Privates

        private IPlayerStateMachine _currentState;

        #endregion
    }
}
