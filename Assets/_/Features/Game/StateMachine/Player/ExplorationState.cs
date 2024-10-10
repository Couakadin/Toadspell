using UnityEngine;
using Data.Runtime;

namespace Game.Runtime
{
    public class ExplorationState : IPlayerStateMachine
    {
        #region Methods

        /// <summary>
        /// CameraManager constructor
        /// </summary>
        /// <param name="player"></param>
        public ExplorationState(PlayerManager player) => this._player = player;

        public void Enter() 
        {
            _player.m_thirdPersonCam.Priority = 10; // Third person activated
            Cursor.visible = false; // Hide cursor
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor in the center of the screen
        }

        public void Exit() { }

        public void Tick()
        {
            if (_player.m_isAiming) _player.SwitchToAimState();
            if (_player.m_isJumping) _player.SwitchToJumpState();
            
            if (_player.m_moveDirection == Vector2.zero) return;
            _player.transform.position += new Vector3(_player.m_moveDirection.x, 0, _player.m_moveDirection.y) * (_player.m_speed * Time.deltaTime);
        } 

        #endregion

        #region Privates

        private PlayerManager _player;

        #endregion
    }
}