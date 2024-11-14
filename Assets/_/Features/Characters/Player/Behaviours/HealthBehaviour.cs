using Data.Runtime;
using UnityEngine;

namespace Player.Runtime
{
    public class HealthBehaviour : MonoBehaviour, ICanBeHurt
    {
        #region Publics

        public int m_startLives;

        #endregion


        #region Unity API

        private void Start()
        {
            _currentLives = m_startLives;
        }

        #endregion


        #region Main Methods

        public void TakeDamage(int damage)
        {
            UpdateLives(damage);
        }

        public void UpdateLives(int lives)
        {
            _currentLives += lives;
            if(_currentLives > 0)
            {
                _playerBlackboard.SetValue("Lives", _currentLives);
                _onUpdatingLife.Raise();
            }
            if(_currentLives == 0)
            {
                _playerBlackboard.SetValue("Lives", _currentLives);
                _onUpdatingLife.Raise();
                _onPlayerDeath.Raise();
            }
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private int _currentLives;
        [SerializeField] private Blackboard _playerBlackboard;

        [Header("life Events")]
        [SerializeField] private VoidEvent _onUpdatingLife;
        [SerializeField] private VoidEvent _onPlayerDeath;

        #endregion
    }
}