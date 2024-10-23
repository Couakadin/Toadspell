using UnityEngine;

public class IdleState : IState
{
    public StateMachine m_stateMachine { get; }

    public IdleState(StateMachine stateMachine)
    {
        m_stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log(GetType().Name);
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
        if (_playerInputMove.sqrMagnitude > .1f) m_stateMachine.ChangeState(m_stateMachine.m_moveState);
        if (_playerInputJump) m_stateMachine.ChangeState(m_stateMachine.m_jumpState);
    }

    public void HandleInput()
    {
        ReadMoveValue();
        IsJumping();
    }

    private void ReadMoveValue()
    {
        _playerInputMove = m_stateMachine.m_playerInput.actions["Move"].ReadValue<Vector2>();
    }

    private void IsJumping()
    {
        _playerInputJump = m_stateMachine.m_playerInput.actions["Jump"].triggered;
    }

    private Vector2 _playerInputMove;

    private bool _playerInputJump;
}
