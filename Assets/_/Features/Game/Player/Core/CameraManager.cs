using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using Data.Runtime;

namespace Player.Runtime
{
    public class CameraManager : MonoBehaviour
    {
        #region Publics 

        public CinemachineFreeLook m_thirdPersonCam;
        public CinemachineVirtualCamera m_shoulderCam;
        public GameObject m_aimReticle;
        public GameObject m_player;

        #endregion

        #region Unity

        private void Start()
        {
            _stateMachine = new PlayerStateMachine();

            // States init
            _explorationState = new ExplorationState(this, m_thirdPersonCam);
            _aimState = new AimState(this, m_shoulderCam, m_aimReticle);

            // Define initial state
            _stateMachine.SetState(_explorationState);
        }

        private void OnEnable()
        {
            _inputReader.AimEvent += HandleAim;
        }

        private void OnDisable()
        {
            _inputReader.AimEvent -= HandleAim;
        }

        private void Update() => _stateMachine.Tick();

        #endregion

        #region Methods

        public void SwitchToExplorationState() => _stateMachine.SetState(_explorationState);

        public void SwitchToAimState() => _stateMachine.SetState(_aimState);

        public bool IsAiming() => _isAiming;

        #endregion

        #region Utils

        private void HandleAim() => _isAiming = _isAiming ? false : true;

        #endregion

        #region Privates

        [Title("Input")]
        [SerializeField]
        private InputReader _inputReader;

        [Title("Privates")]
        private PlayerStateMachine _stateMachine;
        private ExplorationState _explorationState;
        private AimState _aimState;

        private bool _isAiming;

        #endregion
    }
}