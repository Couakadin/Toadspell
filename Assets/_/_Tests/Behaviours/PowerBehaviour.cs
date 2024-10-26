using UnityEngine;
using UnityEngine.InputSystem;

public class PowerBehaviour : MonoBehaviour
{
    #region Publics

    [Header("Lock Params")]
    [Tooltip("The sideview delimiter to catch lockable targets.")]
    public float m_detectionAngle;
    [Tooltip("The range to detect lockable targets.")]
    public float m_detectionRadius;
    [Tooltip("The layer of lockable targets.")]
    public LayerMask m_detectionLayer;

    public InputAction m_lockInput { get; private set; }

    #endregion

    #region Unity

    private void Awake()
    {
        _gameInput = new GameInput();
        _gameplayInput = _gameInput.Gameplay;
        m_lockInput = _gameplayInput.Lock;
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
