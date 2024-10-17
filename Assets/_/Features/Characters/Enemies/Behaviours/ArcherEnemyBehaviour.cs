using Data.Runtime;
using UnityEngine;

namespace Enemies.Runtime
{
    public class ArcherEnemyBehaviour : EnemyBaseBehaviour
    {
        #region Publics

        #endregion


        #region Unity API

        void Start()
    	{
            _attackTimer = new Timer(_attackDelay);
            _attackTimer.OnTimerFinished += Attack;
        }


        void Update()
    	{
            Vector3 playerPosition = m_blackboard.GetValue<Vector3>("Position"); //Cherche constamment le player
            var distanceWithPlayer = (playerPosition - transform.position).magnitude; //distance avec le player

            if (distanceWithPlayer < _maxDetectionRange)
            {
                transform.LookAt(playerPosition);

                if (!_attackTimer.IsRunning())
                {
                    _attackTimer.Reset();
                    _attackTimer.Begin();
                    Debug.Log("Test Timer");
                }
            }
            if (_attackTimer.IsRunning()) _attackTimer.Tick();

        }

        #endregion


        #region Main Methods
        public override void OnLock()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUnlock()
        {
            throw new System.NotImplementedException();
        }

        public override void TakeDamage(float damage)
        {
            throw new System.NotImplementedException();
        }

        public override void Attack()
        {
            Debug.Log("Attack");
        }

        #endregion


        #region Utils

        #endregion


        #region Privates & Protected

        #endregion
    }
}