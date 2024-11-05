using Data.Runtime;
using UnityEngine;

namespace Player.Runtime
{
    public class HealthBehaviour : MonoBehaviour, ICanBeHurt
    {
        #region Unity API

        private void Awake()
        {
            _currentLives = _startLives;
        }

        private void Start()
        {
            _playerBlackboard.SetValue("Lives", _currentLives);
        }

        #endregion


        #region Main Methods

        public void TakeDamage(float damage)
        {
            UpdateLives(damage);
        }

        public void UpdateLives(float lives)
        {
            _currentLives += lives;
            _playerBlackboard.SetValue("Lives", _currentLives);
            _onUpdatingLife.Raise();
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private float _startLives;
        [SerializeField] private float _currentLives;
        [SerializeField] private VoidEvent _onUpdatingLife;
        [SerializeField] private Blackboard _playerBlackboard;

        #endregion
    }
}