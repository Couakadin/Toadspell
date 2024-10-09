using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    public class AimState : IPlayerStateMachine
    {
        #region Methods

        /// <summary>
        /// AimState constructor
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="shoulderCam"></param>
        /// <param name="aimReticle"></param>
        /// <param name="playerInput"></param>
        public AimState(CameraManager manager, CinemachineVirtualCamera shoulderCam, GameObject aimReticle, PlayerInput playerInput)
        {
            this._manager = manager;
            this._shoulderCam = shoulderCam;
            this._aimReticle = aimReticle;
            this._playerInput = playerInput;
        }

        public void Enter()
        {
            _shoulderCam.Priority = 10; // Activate shoulder cam
            _aimReticle.SetActive(true); // Show visor
            _playerInput.enabled = false; // Deactivate Player Input system
            Cursor.visible = true; // Show cursor
            Cursor.lockState = CursorLockMode.None; // Unlock cursor
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _manager.SwitchToExplorationState();
        }

        public void Exit()
        {
            _shoulderCam.Priority = 0; // Deactivate shoulder cam
            _aimReticle.SetActive(false); // Hide visor
        }

        #endregion

        #region Privates

        private CameraManager _manager;
        private CinemachineVirtualCamera _shoulderCam;
        private GameObject _aimReticle;
        private PlayerInput _playerInput;

        #endregion
    }
}
