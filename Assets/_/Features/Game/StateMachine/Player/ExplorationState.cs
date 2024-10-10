using UnityEngine;
using Data.Runtime;

namespace Game.Runtime
{
    public class ExplorationState : IPlayerStateMachine
    {
        #region Methods

        /// <summary>
        /// PlayerManager constructor
        /// </summary>
        /// <param name="player"></param>
        public ExplorationState(PlayerManager player) => this._player = player;

        public void Enter() 
        {
            _player.m_thirdPersonCam.Priority = 10; // Third person activated
        }

        public void Exit() 
        {
            _player.m_thirdPersonCam.Priority = 0; // Third person deactivated
        }

        public void Tick()
        {
            // State switching logic
            if (_player.m_isAiming) _player.SwitchToAimState();
            if (_player.m_isJumping) _player.SwitchToJumpState();

            // Check for movement input
            if (_player.m_moveDirection == Vector2.zero) return;

            // Move the player
            Vector3 move = new Vector3(_player.m_moveDirection.x, 0, _player.m_moveDirection.y);
            _player.transform.position += move * (_player.m_speed * Time.deltaTime);

            // Rotate the player towards the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(move);
            _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        } 

        #endregion

        #region Privates

        private PlayerManager _player;

        private readonly float _rotationSpeed = 10f;

        #endregion
    }
}