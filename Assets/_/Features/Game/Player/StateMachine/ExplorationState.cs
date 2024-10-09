using UnityEngine;
using Cinemachine;

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
        public ExplorationState(CameraManager manager, CinemachineFreeLook thirdPersonCam)
        {
            this._manager = manager;
            this._thirdPersonCam = thirdPersonCam;
        }

        public void Enter()
        {
            _thirdPersonCam.Priority = 10; // Third person activated
            Cursor.visible = false; // Hide cursor
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor in the center of the screen
        }

        public void Tick()
        {
            if (_manager.IsAiming()) _manager.SwitchToAimState();
        }

        public void Exit() => _thirdPersonCam.Priority = 0; // Third person deactivated

        #endregion

        #region Privates

        private CameraManager _manager;
        private CinemachineFreeLook _thirdPersonCam;

        #endregion
    }
}