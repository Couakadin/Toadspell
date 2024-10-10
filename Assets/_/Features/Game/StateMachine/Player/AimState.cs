using UnityEngine;
using Data.Runtime;

namespace Game.Runtime
{
    public class AimState : IPlayerStateMachine
    {
        #region Methods

        /// <summary>
        /// AimState constructor
        /// </summary>
        /// <param name="player"></param>
        public AimState(PlayerManager player) => this._player = player;

        public void Enter()
        {
            _player.m_shoulderCam.Priority = 10; // Activate shoulder cam
            _player.m_aimReticle.SetActive(true); // Show visor
        }

        public void Exit()
        {
            _player.m_shoulderCam.Priority = 0; // Deactivate shoulder cam
            _player.m_aimReticle.SetActive(false); // Hide visor
        }

        public void Tick()
        {
            if (!_player.m_isAiming) _player.SwitchToExplorationState();
        }

        #endregion

        #region Privates

        private PlayerManager _player;

        #endregion
    }
}
