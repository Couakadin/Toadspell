using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockState : IState
{
    #region Methods

    public StateMachine m_stateMachine { get; }

    public GameObject m_currentLockTarget { get; private set; }

    public LockState(StateMachine stateMachine)
    {
        m_stateMachine = stateMachine;

        // Player
        _playerTransform = m_stateMachine.m_powerBehaviour.transform;

        // Input
        _lockInput = m_stateMachine.m_powerBehaviour.m_lockInput;
        _tongueInput = m_stateMachine.m_powerBehaviour.m_tongueInput;

        // Lock
        _detectionRadius = m_stateMachine.m_powerBehaviour.m_detectionRadius;
        _detectionLayer = m_stateMachine.m_powerBehaviour.m_detectionLayer;

        // Camera
        _cameraMain = Camera.main;
        _cameraTransform = _cameraMain.transform;
    }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        m_currentLockTarget = _currentLockedTarget;
    }

    public void Tick()
    {
        UpdateLockableList();
    }

    public void PhysicsTick()
    {
        
    }

    public void FinalTick()
    {
        
    }

    public void HandleInput()
    {
        if (_lockInput.triggered) SwitchTarget();
        if (_tongueInput.triggered) m_stateMachine.ChangeState(m_stateMachine.m_tongueState);
    }

    #endregion

    #region Utils

    /// <summary>
    /// Update the list of lockable objects within the detection radius and a 45-degree cone in front of the player.
    /// </summary>
    private void UpdateLockableList()
    {
        // Obtenir tous les colliders dans le rayon de détection
        _hitColliders = Physics.OverlapSphere(_playerTransform.position, _detectionRadius, _detectionLayer);

        // Effacer la liste existante
        _lockingList.Clear();

        // Boucle à travers les colliders détectés
        foreach (Collider hitCollider in _hitColliders)
        {
            // Vérifie si l'objet est visible dans l'écran de la caméra
            _viewportPoint = _cameraMain.WorldToViewportPoint(hitCollider.transform.position);

            // Vérifie si le point est dans le champ de vision (dans les limites de l’écran)
            _isInView = _viewportPoint.z > 0 &&
                            _viewportPoint.x > 0 && _viewportPoint.x < 1 &&
                            _viewportPoint.y > 0 && _viewportPoint.y < 1;

            // Si l'objet est visible à l'écran et implémente ILockable, ajoute-le à la liste
            if (_isInView && hitCollider.TryGetComponent<ILockable>(out ILockable lockable)) _lockingList.Add(hitCollider.gameObject);
        }

        // Si aucune cible n'est trouvée, déverrouille la cible actuellement verrouillée
        if (_lockingList.Count == 0) UnlockTarget();
        // Si des cibles sont disponibles, verrouille la première si aucune cible n'est verrouillée
        else if (_currentLockedTarget == null)
        {
            LockTarget(_lockingList[0]);
            _currentTargetIndex = 0;
        }
    }

    /// <summary>
    /// Switch to the next target in the locking list when Tab is pressed.
    /// </summary>
    private void SwitchTarget()
    {
        if (_lockingList.Count <= 1) return; // If only one or no target is in the list, don't switch

        // Increment the target index and loop back to the start if necessary
        _currentTargetIndex = (_currentTargetIndex + 1) % _lockingList.Count;

        // Lock the new target
        LockTarget(_lockingList[_currentTargetIndex]);
    }

    /// <summary>
    /// Lock the specified target and change its material to indicate it's locked.
    /// </summary>
    /// <param name="target"></param>
    private void LockTarget(GameObject target)
    {
        // If the target is already locked, do nothing
        if (_currentLockedTarget == target) return;

        // Unlock the previous target
        UnlockTarget();

        // Set the new target and lock it
        _currentLockedTarget = target;
        target.TryGetComponent<ILockable>(out ILockable lockable);
        if (lockable == null) return;
        lockable.OnLock(); // Call the lock behavior (e.g., change material to red)
    }

    /// <summary>
    /// Unlock the currently locked target.
    /// </summary>
    private void UnlockTarget()
    {
        // If there's no currently locked target, do nothing
        if (_currentLockedTarget == null) return;

        // Call the OnUnlock method of the currently locked target
        _currentLockedTarget.TryGetComponent<ILockable>(out ILockable lockable);
        if (lockable == null) return;
        lockable.OnUnlock(); // Call the unlock behavior (e.g., revert material to default)

        // Clear the current locked target
        _currentLockedTarget = null;
    }

    #endregion

    #region Privates

    // Lists
    private List<GameObject> _lockingList = new(); // List of detected lockable objects

    private GameObject _currentLockedTarget = null;
    private int _currentTargetIndex = -1; // Index of the currently locked target in the list

    // Player
    private Transform _playerTransform;
    private Collider[] _hitColliders;

    // Input
    private InputAction _lockInput;
    private InputAction _tongueInput;
    
    // Lock
    private float _detectionRadius;
    private LayerMask _detectionLayer;

    // Camera
    private Camera _cameraMain;
    private Transform _cameraTransform;
    private bool _isInView;
    private Vector3 _viewportPoint;

    #endregion
}
