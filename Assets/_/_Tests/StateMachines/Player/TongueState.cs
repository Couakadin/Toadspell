using UnityEngine;

public class TongueState : IState
{
    #region Methods

    public StateMachine m_stateMachine { get; }

    public TongueState(StateMachine stateMachine)
    {
        m_stateMachine = stateMachine;

        // Player
        _playerTransform = m_stateMachine.m_powerBehaviour.transform;
        _tongueMaxDistance = m_stateMachine.m_powerBehaviour.m_tongueMaxDidtsance;

        // Lock
        _detectionLayer = m_stateMachine.m_powerBehaviour.m_detectionLayer;
    }

    public void Enter()
    {
        _currentLockTarget = m_stateMachine.m_lockState.m_currentLockTarget?.transform;
        if (_currentLockTarget == null) m_stateMachine.ChangeState(m_stateMachine.m_lockState);
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        DetectTarget();   
    }

    public void PhysicsTick()
    {
        
    }

    public void FinalTick()
    {
        
    }

    public void HandleInput()
    {
        
    }

    #endregion

    #region Utils

    private void DetectTarget()
    {
        // Normalized direction between player and current lock target
        _directionToTarget = (_currentLockTarget.position - _playerTransform.position).normalized;

        // Raycast to the current lock target
        if (Physics.Raycast(_playerTransform.position, _directionToTarget, out _hit, _tongueMaxDistance, _detectionLayer))
        {
            Debug.Log(_hit.collider.name);
            Debug.DrawRay(_playerTransform.position, _directionToTarget * _tongueMaxDistance, Color.red);
        }
    }

    #endregion

    #region Privates

    // Player
    private Transform _playerTransform;
    private float _tongueMaxDistance;

    // Lock
    private Transform _currentLockTarget;
    private LayerMask _detectionLayer;

    // Physics
    private RaycastHit _hit;
    private Vector3 _directionToTarget;

    #endregion
}
