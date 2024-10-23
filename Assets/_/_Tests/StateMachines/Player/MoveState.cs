using UnityEngine;

public class MoveState : IState
{
    public StateMachine m_stateMachine { get; }

    public MoveState(StateMachine stateMachine)
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
        HandleMove();
    }

    public void FinalTick()
    {
        if (_playerInputMove.sqrMagnitude < .1f) m_stateMachine.ChangeState(m_stateMachine.m_idleState);
        if (_playerInputJump) m_stateMachine.ChangeState(m_stateMachine.m_jumpState);
    }

    public void HandleInput()
    {
        ReadMoveValue();
        IsJumping();
    }

    private void HandleMove()
    {
        _playerMoveDirection = new Vector3(_playerInputMove.x, 0, _playerInputMove.y);
        m_stateMachine.m_characterController.SimpleMove(_speed * _playerMoveDirection);
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
    private Vector3 _playerMoveDirection;

    private bool _playerInputJump;

    private float _speed = 5f;
}
