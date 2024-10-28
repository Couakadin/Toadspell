using UnityEngine;
using UnityEngine.InputSystem;

public class PowerBehaviour : MonoBehaviour
{
    #region Publics

    [Header("Lock Params")]
    [Tooltip("The range to detect lockable targets.")]
    public float m_detectionRadius;
    [Tooltip("The layer of lockable targets.")]
    public LayerMask m_detectionLayer;

    [Header("Tongue Params")]
    [Tooltip("The GameObject of the tongue.")]
    public GameObject m_tongue;
    [Tooltip("The speed of the tongue.")]
    public float m_tongueSpeed;

    public InputAction m_lockInput { get; private set; }
    public InputAction m_tongueInput { get; private set; }
    public InputAction m_moveInput {  get; private set; }

    #endregion

    #region Unity

    private void Awake()
    {
        _gameInput = new GameInput();
        _gameplayInput = _gameInput.Gameplay;
        m_lockInput = _gameplayInput.Lock;
        m_tongueInput = _gameplayInput.Tongue;
        m_moveInput = _gameplayInput.Move;
    }

    private void Start()
    {
        _stateMachine = new(this);
        _stateMachine.ChangeState(_stateMachine.m_lockState);
    }

    private void OnEnable()
    {
        _gameInput.Enable();
    }

    private void OnDisable()
    {
        _gameInput.Disable();
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

    #endregion

    #region Privates

    private StateMachine _stateMachine;

    private GameInput _gameInput;
    private GameInput.GameplayActions _gameplayInput;

    #endregion
}
