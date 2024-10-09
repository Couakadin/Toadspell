using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

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
            _playerInput = m_player.GetComponent<PlayerInput>();
            _stateMachine = new PlayerStateMachine();

            // States init
            _explorationState = new ExplorationState(this, m_thirdPersonCam, _playerInput);
            _aimState = new AimState(this, m_shoulderCam, m_aimReticle, _playerInput);

            // Define initial state
            _stateMachine.SetState(_explorationState);
        }

        private void Update() => _stateMachine.Tick();

        #endregion

        #region Methods

        public void SwitchToExplorationState() => _stateMachine.SetState(_explorationState);

        public void SwitchToAimingState() => _stateMachine.SetState(_aimState);

        #endregion

        #region Privates

        private PlayerStateMachine _stateMachine;
        private ExplorationState _explorationState;
        private AimState _aimState;

        private PlayerInput _playerInput;

        #endregion
    }
}