using Data.Runtime;
using UnityEngine;

namespace Player.Runtime
{
    public class HealthBehaviour : MonoBehaviour, ICanBeHurt
    {
        #region Publics

        #endregion


        #region Unity API

        private void Awake()
        {
            _currentLives = _startLives;
            _playerBlackboard.SetValue("Lives", _currentLives);
        }

    	void Update()
    	{
	
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

        [ContextMenu("test losing lives")]
        public void HurtByEnvironment()
        {
            UpdateLives(-1);
        }

        #endregion


        #region Utils

        #endregion


        #region Privates & Protected

        [SerializeField] private float _startLives;
        [SerializeField] private float _currentLives;
        [SerializeField] private VoidEvent _onUpdatingLife;
        [SerializeField] private Blackboard _playerBlackboard;


        #endregion
    }
}