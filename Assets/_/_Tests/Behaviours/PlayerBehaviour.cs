using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [HideInInspector]
    public PlayerInput m_playerInput { get; private set; }
    [HideInInspector]
    public CharacterController m_characterController { get; private set; }

    private void Awake()
    {
        TryGetComponent(out _characterController);
        m_characterController = _characterController;

        TryGetComponent(out _playerInput);
        m_playerInput = _playerInput;
    }

    private void Start()
    {
        _stateMachine = new(this);
        _stateMachine.ChangeState(_stateMachine.m_idleState);
    }

    private void Update()
    {
        _stateMachine.HandleInput();
        _stateMachine.Tick();
    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsTick();
    }

    private void LateUpdate()
    {
        _stateMachine.FinalTick();
    }

    private CharacterController _characterController;
    private StateMachine _stateMachine;
    private PlayerInput _playerInput;
}
