namespace Data.Runtime
{
    /// <summary>
    /// This is an interface for a State Machine.
    /// Defines the methods that any state should implement.
    /// </summary>
    public interface IState
    {
        #region Main
        /// <summary>
        /// Called when the state is entered.
        /// Used to initialize variables or set up the state.
        /// </summary>
        public void Enter();
        /// <summary>
        /// Called when the state is exited.
        /// Used for cleanup or resetting values before transitioning to another state.
        /// </summary>
        public void Exit();
        /// <summary>
        /// Handles player input specific to this state.
        /// Called during the input phase of the frame.
        /// </summary>
        public void HandleInput();
        /// <summary>
        /// Called on every frame to update the logic of the state.
        /// Used for frame-by-frame updates, such as movement or state transitions.
        /// </summary>
        public void Tick();
        /// <summary>
        /// Called during the physics update cycle.
        /// Used for handling any physics-based calculations or movement.
        /// </summary>
        public void PhysicsTick();
        /// <summary>
        /// Called during the LateUpdate cycle.
        /// Used for final adjustments that require the final state of all objects.
        /// </summary>
        public void FinalTick();
        #endregion
    }
}