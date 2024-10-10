using Cinemachine;
using Data.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Runtime
{
    public class PlayerManager : MonoBehaviour
    {
        #region Publics 

        public CinemachineFreeLook m_thirdPersonCam;
        public CinemachineVirtualCamera m_shoulderCam;
        public GameObject m_aimReticle;

        [HideInInspector]
        public Vector2 m_moveDirection;

        [HideInInspector]
        public float m_speed, m_jumpSpeed;
        [HideInInspector]
        public bool m_isJumping, m_isAiming;

        #endregion

        #region Unity

        private void Awake()
        {
            m_speed = _playerBlackboard.GetValue<float>("Speed");
            m_jumpSpeed = _playerBlackboard.GetValue<float>("JumpSpeed");
        }

        private void Start()
        {
            _stateMachine = new();

            // States init
            _explorationState = new(this);
            _aimState = new(this);
            _jumpState = new(this);

            // Define initial state
            _stateMachine.SetState(_explorationState);
        }

        private void OnEnable()
        {
            _inputReader.MoveEvent += HandleMove;

            _inputReader.JumpEvent += HandleJump;
            _inputReader.JumpCancelledEvent += HandleCancelledJump;

            _inputReader.AimEvent += HandleAim;
        }

        private void OnDisable()
        {
            _inputReader.MoveEvent -= HandleMove;

            _inputReader.JumpEvent -= HandleJump;
            _inputReader.JumpCancelledEvent -= HandleCancelledJump;

            _inputReader.AimEvent -= HandleAim;
        }

        private void Update() => _stateMachine.Tick();

        #endregion

        #region Methods

        public void SwitchToExplorationState() => _stateMachine.SetState(_explorationState);

        public void SwitchToAimState() => _stateMachine.SetState(_aimState);

        public void SwitchToJumpState() => _stateMachine.SetState(_jumpState);

        #endregion

        #region Utils

        private void HandleMove(Vector2 dir) => m_moveDirection = dir;

        private void HandleJump() => m_isJumping = true;
        private void HandleCancelledJump() => m_isJumping = false;

        private void HandleAim() => m_isAiming = m_isAiming ? false : true;

        #endregion

        #region Privates

        [Title("Blackboard")]
        [SerializeField]
        private Blackboard _playerBlackboard;

        [Title("Input")]
        [SerializeField]
        private InputReader _inputReader;

        [Title("Privates")]
        private PlayerStateMachine _stateMachine;
        private ExplorationState _explorationState;
        private AimState _aimState;
        private JumpState _jumpState;

        #endregion
    }
}
