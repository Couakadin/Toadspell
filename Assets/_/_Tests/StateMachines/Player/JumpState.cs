using UnityEngine;

public class JumpState : IState
{
    public StateMachine m_stateMachine { get; }

    public JumpState(StateMachine stateMachine)
    {
        m_stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log(GetType().Name);
        _isGrounded = m_stateMachine.m_characterController.isGrounded;
    }

    public void Exit()
    {

    }

    public void Tick()
    {
        HandleJump();
    }

    public void PhysicsTick()
    {
        
    }

    public void FinalTick()
    {
        if (!_playerInputJump) m_stateMachine.ChangeState(m_stateMachine.m_idleState);
    }

    public void HandleInput()
    {
        IsJumping();
    }

    private void HandleJump()
    {
        Debug.Log($"Grounded: {_isGrounded}"); Debug.Log($"Jumping: {_playerInputJump}");
        if (_isGrounded && _verticalVelocity.y < 0)
        {
            _verticalVelocity.y = 0;
        }

        if (_isGrounded)
        {
            Debug.Log(2);
            _verticalVelocity.y = _jumpForce;
        }

        _verticalVelocity.y += Time.deltaTime * Physics.gravity.y;

        m_stateMachine.m_characterController.Move(_verticalVelocity);
    }

    private void IsJumping()
    {
        _playerInputJump = m_stateMachine.m_playerInput.actions["Jump"].triggered;
    }

    private Vector3 _verticalVelocity;

    private bool _playerInputJump;
    private bool _isGrounded;

    private float _jumpForce = 15f;
}
