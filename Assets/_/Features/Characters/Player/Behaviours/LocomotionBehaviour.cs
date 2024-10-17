using UnityEngine;
using StateMachine.Runtime;
using Data.Runtime;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.UI;

namespace Player.Runtime
{
    [RequireComponent(typeof(Rigidbody))]
    public class LocomotionBehaviour : MonoBehaviour
    {
        #region Unity

        private void Awake()
        {
            // Settings
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Blackboard Init values
            _playerBlackboard.SetValue<bool>("IsAiming", false);
            _playerBlackboard.SetValue<Vector2>("MoveDirection", Vector2.zero);

            // Aim image
            _aimImage.enabled = false;

            // Components
            TryGetComponent<Rigidbody>(out _rigidbody);

            // Dictionary
            _sharedParameterDictionary.Add("rigidbody", _rigidbody);
            _sharedParameterDictionary.Add("playerBlackboard", _playerBlackboard);
            _sharedParameterDictionary.Add("playerStats", _playerStats);
            _sharedParameterDictionary.Add("thirdPersonCamera", _thirdPersonCamera);
            _sharedParameterDictionary.Add("shoulderCamera", _shoulderCamera);
            _sharedParameterDictionary.Add("transform", transform);
            _sharedParameterDictionary.Add("aimImage", _aimImage);
        }

        private void Start()
        {
            // StateMachine
            _stateMachine = new();

            // Creating states
            ExplorationState explorationState = new(_stateMachine, _inputReader, _sharedParameterDictionary);
            JumpState jumpState = new(_stateMachine, _sharedParameterDictionary);
            AimState aimState = new(_stateMachine, _sharedParameterDictionary);

            // Set the cross-references
            explorationState.SetJumpState(jumpState);
            explorationState.SetAimState(aimState);

            jumpState.SetExplorationState(explorationState);

            aimState.SetExplorationState(explorationState);

            // Initial State
            _stateMachine.SetState(explorationState);
        }

        private void OnEnable()
        {
            _inputReader.ExplorationEvent += HandleExploration;

            _inputReader.AimEvent += HandleAim;
        }

        private void OnDisable()
        {
            _inputReader.ExplorationEvent -= HandleExploration;

            _inputReader.AimEvent -= HandleAim;
        }

        private void Update()
        {
            _stateMachine.Tick();
            // Définit la position de départ et de fin de la ligne
            Vector3 startPosition = transform.position;
            Vector3 endPosition = transform.position + transform.forward * 10f;
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedTick();
        }

        private void LateUpdate()
        {
            _stateMachine.LateTick();
            _playerBlackboard.SetValue<Vector3>("Position", transform.position);
        }

        #endregion

        #region Utils

        private void HandleExploration(Vector2 direction)
        {
            _moveDirection = direction;
            _playerBlackboard.SetValue<Vector2>("MoveDirection", _moveDirection);
        }

        private void HandleAim()
        {
            if (!_playerBlackboard.GetValue<bool>("IsGrounded")) return;
            _isAiming = _isAiming ? false : true;
            _playerBlackboard.SetValue<bool>("IsAiming", _isAiming);
        }

        #endregion

        #region Privates

        // Dictionary
        private Dictionary<string, object> _sharedParameterDictionary = new();

        // StateMachine
        private StateMachineCore _stateMachine;

        // Input
        [Title("ScriptableObjects")]
        [SerializeField]
        private GameInputObject _inputReader;
        // Blackboard
        [SerializeField]
        private Blackboard _playerBlackboard;
        // Base Stats
        [SerializeField]
        private PlayerStatsData _playerStats;

        [Title("Cameras")]
        [SerializeField]
        private CinemachineVirtualCamera _shoulderCamera;
        [SerializeField]
        private CinemachineVirtualCamera _thirdPersonCamera;
        [SerializeField]
        private Image _aimImage;

        // Components
        private Rigidbody _rigidbody;

        // Variables
        private Vector2 _moveDirection;
        private bool _isAiming;

        #endregion
    }
}
