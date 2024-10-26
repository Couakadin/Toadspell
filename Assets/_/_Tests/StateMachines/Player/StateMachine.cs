public class StateMachine : AState
{
    public PowerBehaviour m_powerBehaviour { get; }

    public LockState m_lockState { get; }
    public TongueState m_tongueState { get; }

    public StateMachine(PowerBehaviour powerBehaviour)
    {
        m_powerBehaviour = powerBehaviour;

        m_lockState = new(this);
        m_tongueState = new(this);
    }
}
