using Data.Runtime;
using UnityEngine;

namespace Enemies.Runtime
{
    public abstract class EnemyBaseBehaviour : MonoBehaviour, IAmLockable, IAmInteractable, ICanBeHurt
    {
        #region Publics

        [Header("Enemy Specificities")]
        public float m_attackDelay = 2;
        public float m_lifePoints;
        public float m_maxDetectionRange = 20f;
        public IAmInteractable.Size _enemySize;

        [Header("References")]
        public Blackboard m_blackboard;

        public IAmInteractable.Size m_grapSize => _enemySize;

        public float m_offsetDistance => throw new System.NotImplementedException();

        #endregion


        #region Main Methods

        public abstract void OnLock();

        public abstract void OnUnlock();

        public abstract void Attack();

        public abstract void TakeDamage(float damage);

        #endregion


        #region Utils

        #endregion


        #region Privates & Protected

        protected Timer _attackTimer;

        #endregion
    }
}