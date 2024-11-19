using Data.Runtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies.Runtime
{
    public class ArcherEnemyBehaviour : EnemyBaseBehaviour
    {

        #region Unity API

        void Start()
    	{
            _dissolver = GetComponent<ICanDissolve>();
            _healthBar.maxValue = m_lifePoints;
            _healthBar.value = m_lifePoints;
            _attackTimer = CreateAndSubscribeTimer(m_attackDelay, StartAttacking);
            _damageTimer = CreateAndSubscribeTimer(_takeDamageDelay, ResumeAfterDamage);
        }

        void Update()
    	{
            Vector3 playerPosition = m_blackboard.GetValue<Vector3>("Position");
            Vector3 bookPosition = m_blackboard.GetValue<Vector3>("Contact");
            var distanceWithPlayer = (playerPosition - transform.position).magnitude;

            if (distanceWithPlayer < m_maxDetectionRange)
            {
                SetOrResetTimer(_attackTimer);
                transform.LookAt(playerPosition);
                _mouth.transform.LookAt(bookPosition);
            }

            UpdateTimers();
            if(_isShooting) Attack();
        }

        #endregion


        #region Main Methods

        public override void Attack()
        {
            m_animator.SetBool("Attacking", true);

            if (m_animator.GetAnimatorTransitionInfo(0).IsName("attack -> idle"))
            {
                _isShooting = false;
                m_animator.SetBool("Attacking", false);
            }
        }

        public override void OnLock()
        {
            _LockIndicator.SetActive(true);
        }

        public override void OnUnlock()
        {
            _LockIndicator.SetActive(false);
        }

        public override void TakeDamage(int damage)
        {
            _isShooting = false;
            m_animator.SetBool("Attacking", false);
            m_lifePoints -= damage;
            _healthBar.value = m_lifePoints;
            SetOrResetTimer(_damageTimer);
        }

        private void StartAttacking()
        {
            _isShooting = true;
        }

        #endregion


        #region Utils

        private void ResumeAfterDamage()
        {
            if (m_lifePoints <= 0) _dissolver.StartDissolve();
        }

        private void UpdateTimers()
        {
            if (_attackTimer.IsRunning()) _attackTimer.Tick();
            if (_damageTimer.IsRunning()) _damageTimer.Tick();
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_maxDetectionRange);
        }

        #endregion


        #region Private & Protected

        [SerializeField] private Transform _mouth;
        [SerializeField] private float _takeDamageDelay = .5f;
        [SerializeField] private Slider _healthBar;
        private Color _originalMaterial;
        private bool _isShooting = false;
        private Timer _damageTimer;
        private ICanDissolve _dissolver;


        #endregion
    }
}