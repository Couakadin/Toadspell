using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachine : AState
{
    public CharacterController m_characterController {  get; }
    public PlayerBehaviour m_playerBehaviour { get; }
    public PlayerInput m_playerInput { get; }

    public IdleState m_idleState { get; }
    public MoveState m_moveState { get; }
    public JumpState m_jumpState { get; }

    public StateMachine(PlayerBehaviour playerBehaviour)
    {
        m_playerBehaviour = playerBehaviour;
        m_playerInput = m_playerBehaviour.m_playerInput;
        m_characterController = m_playerBehaviour.m_characterController;

        m_idleState = new(this);
        m_moveState = new(this);
        m_jumpState = new(this);
    }
}
