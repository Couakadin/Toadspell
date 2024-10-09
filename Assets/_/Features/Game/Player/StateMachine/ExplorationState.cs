using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    public class ExplorationState : IPlayerStateMachine
    {
        #region Methods

        /// <summary>
        /// CameraManager constructor
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="thirdPersonCam"></param>
        /// <param name="playerInput"></param>
        public ExplorationState(CameraManager manager, CinemachineFreeLook thirdPersonCam, PlayerInput playerInput)
        {
            this._manager = manager;
            this._thirdPersonCam = thirdPersonCam;
            this._playerInput = playerInput;
        }

        public void Enter()
        {
            _thirdPersonCam.Priority = 10; // Third person activated
            _playerInput.enabled = true; // Player Input system activated
            Cursor.visible = false; // Hide cursor
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor in the center of the screen
        }

        public void Tick()
        {
            // Transition vers l'état Aiming lorsqu'on appuie sur Espace
            if (Input.GetKeyDown(KeyCode.Space))
                _manager.SwitchToAimingState();
        }

        public void Exit() => _thirdPersonCam.Priority = 0; // Third person deactivated

        #endregion

        #region Privates

        private CameraManager _manager;
        private CinemachineFreeLook _thirdPersonCam;
        private PlayerInput _playerInput;

        #endregion
    }
}