using Data.Runtime;
using UnityEngine;

namespace Game.Runtime
{
    public class JumpState : IPlayerStateMachine
    {
        public JumpState(PlayerManager player) => this._player = player;

        public void Enter()
        {
            _player.m_thirdPersonCam.Priority = 10; // Third person activated
        }

        public void Exit()
        {
            
        }

        public void Tick()
        {
            if (_player.m_isJumping)
                _player.transform.position += new Vector3(0, 1, 0) * (_player.m_jumpSpeed * Time.deltaTime);
            else _player.SwitchToExplorationState();
        }

        #region Privates

        private PlayerManager _player;

        #endregion
    }
}
