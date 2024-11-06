using Data.Runtime;
using UnityEngine;
using System;

namespace Enemies.Runtime
{
    public abstract class EnemyBaseBehaviour : MonoBehaviour, IAmLockable, ICanBeHurt
    {
        #region Publics

        [Header("References")]
        public Blackboard m_blackboard;
        public Animator m_animator;
        private GameObject _LockIndicator;

        [Header("Enemy Specificities")]
        public float m_attackDelay = 2;
        public float m_lifePoints;
        public float m_damages = 1;
        public float m_maxDetectionRange = 20f;

        public float m_offsetDistance => throw new System.NotImplementedException();

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