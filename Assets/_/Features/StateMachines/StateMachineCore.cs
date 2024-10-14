using Data.Runtime;

namespace StateMachine.Runtime
{
    public class StateMachineCore
    {
        #region Methods

        /// <summary>
        /// Set a new state of a StateMachine instance.
        /// </summary>
        /// <param name="newState"></param>
        public void SetState(IAmStateMachine newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        /// <summary>
        /// Get the current state of a State Machine.
        /// </summary>
        /// <returns></returns>
        public IAmStateMachine GetState() => _currentState;

        /// <summary>
        /// Similar to Update method, call each frame.
        /// </summary>
        public void Tick() => _currentState?.Tick();

        /// <summary>
        /// Similar to Update method, call each fixed frame.
        /// </summary>
        public void FixedTick() => _currentState?.FixedTick();

        /// <summary>
        /// Similar to Update method, call each end of frame.
        /// </summary>
        public void LateTick() => _currentState?.LateTick();

        #endregion

        #region Privates

        // Parameters
        private IAmStateMachine _currentState;

        #endregion
    }
}
