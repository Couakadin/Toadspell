using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.Runtime;

namespace Enemies.Runtime
{
    public abstract class EnemyBaseBehaviour : MonoBehaviour, IAmLockable, IAmInteractable, ICanBeHurt
    {
        #region Publics

        public Blackboard m_blackboard;
        public IAmInteractable.Size m_grapSize => throw new System.NotImplementedException();

        #endregion


        #region Unity API

        void Start()
    	{
            
    }

    	void Update()
    	{
	
    	}

        #endregion


        #region Main Methods

        public abstract void Attack();

        public abstract void OnLock();

        public abstract void OnUnlock();

        public abstract void TakeDamage(float damage);

        #endregion


        #region Utils

        #endregion


        #region Privates & Protected

        protected Timer _attackTimer;
        protected float _attackDelay = 2;
        protected float _maxDetectionRange = 20f;

        #endregion
    }
}