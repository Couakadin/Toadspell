using Data.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
        }


        void Update()
    	{
            Vector3 playerPosition = m_blackboard.GetValue<Vector3>("Position"); //Cherche constamment le player
            var distanceWithPlayer = (playerPosition - transform.position).magnitude; //distance avec le player

            if (distanceWithPlayer < _maxDetectionRange)
            {
                transform.LookAt(playerPosition);

                _attackTimer.Reset();
                _attackTimer.Begin();

                if (_attackTimer.IsRunning()) return;
                Attack();
            }

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
            throw new System.NotImplementedException();
        }

        #endregion


        #region Utils

        #endregion


        #region Privates & Protected

        #endregion
    }
}