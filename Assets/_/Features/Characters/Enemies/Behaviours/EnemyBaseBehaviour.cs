using Data.Runtime;
using UnityEngine;
using System;

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
        private GameObject _LockIndicator;

        public IAmInteractable.Size m_grapSize => _enemySize;

        public float m_offsetDistance => throw new System.NotImplementedException();

        public IAmInteractable.SpellType spellType => throw new NotImplementedException();

        #endregion


        #region Main Methods

        public abstract void OnLock();

        public abstract void OnUnlock();

        public abstract void Attack();

        public abstract void TakeDamage(float damage);



        #endregion


        #region Utils

        public Timer CreateAndSubscribeTimer(float delay, Action callback)
        {
            Timer timer = new Timer(delay);
            timer.OnTimerFinished += callback;
            return timer;
        }

        public void SetOrResetTimer(Timer timer)
        {
            if (!timer.IsRunning())
            {
                timer.Reset();
                timer.Begin();
            }
        }

        #endregion


        #region Privates & Protected

        protected Timer _attackTimer;

        #endregion
    }
}