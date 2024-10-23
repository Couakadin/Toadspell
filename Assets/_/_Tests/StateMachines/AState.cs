/// <summary>
/// Abstract base class for managing states in a State Machine.
/// Handles transitions between states and provides methods for updating them.
/// </summary>
public abstract class AState
{
    #region Publics
    /// <summary>
    /// The current active state of the State Machine.
    /// </summary>
    public IState m_currentState { get; protected set; }
    #endregion

    #region Main
    /// <summary>
    /// Changes the current state to a new state.
    /// Calls Exit() on the old state and Enter() on the new state.
    /// </summary>
    /// <param name="newState">The new state to transition into.</param>
    public void ChangeState(IState newState)
    {
        m_currentState?.Exit();
        m_currentState = newState;
        m_currentState?.Enter();
    }
    /// <summary>
    /// Handles input for the current state.
    /// Calls HandleInput() on the current state.
    /// </summary>
    public void HandleInput() => m_currentState?.HandleInput();
    /// <summary>
    /// Updates the logic for the current state every frame.
    /// Calls Tick() on the current state.
    /// </summary>
    public void Tick() => m_currentState?.Tick();
    /// <summary>
    /// Updates the physics for the current state in the physics update cycle.
    /// Calls PhysicsTick() on the current state.
    /// </summary>
    public void PhysicsTick() => m_currentState?.PhysicsTick();
    /// <summary>
    /// Updates the final state logic, such as camera or animations, during the LateUpdate cycle.
    /// Calls FinalTick() on the current state.
    /// </summary>
    public void FinalTick() => m_currentState?.FinalTick();
    #endregion
}
