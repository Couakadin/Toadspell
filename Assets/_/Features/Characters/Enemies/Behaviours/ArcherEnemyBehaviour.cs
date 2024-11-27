using Data.Runtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies.Runtime
{
    public class ArcherEnemyBehaviour : EnemyBaseBehaviour, ICanBeImmobilized
    {

        #region Unity API

        void Start()
    	{
            _dissolver = GetComponent<ICanDissolve>();
            _healthBar.maxValue = m_lifePoints;
            _healthBar.value = m_lifePoints;
            _attackTimer = CreateAndSubscribeTimer(m_attackDelay, StartAttacking);
        }

        void Update()
    	{
            if (_isFrozen) return;

            Vector3 playerPosition = m_blackboard.GetValue<Vector3>("Position");
            Vector3 bookPosition = m_blackboard.GetValue<Vector3>("Contact");
            var distanceWithPlayer = (playerPosition - transform.position).magnitude;

            if (distanceWithPlayer < m_maxDetectionRange)
            {
                SetOrResetTimer(_attackTimer);
                Quaternion lookOnLoook = Quaternion.LookRotation(playerPosition -  transform.position);
                //transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLoook, _rotationSpeed * Time.deltaTime);
                //transform.LookAt(playerPosition);
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
            if (m_lifePoints <= 0)
            {
                ResumeAfterDamage();
            }
        }

        private void StartAttacking()
        {
            _isShooting = true;
        }

        #endregion


        #region Utils

        private void ResumeAfterDamage()
        {
            _isFrozen = true;
            _enemySound.PlaySoundWhenDying();
            _dissolver.StartDissolve();

        }

        private void UpdateTimers()
        {
            if (_attackTimer.IsRunning()) _attackTimer.Tick();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_maxDetectionRange);
        }

        public void FreezePosition()
        {
            _isFrozen = true;
        }

        public void UnFreezePosition()
        {
            _isFrozen = false;
        }

        #endregion


        #region Private & Protected

        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _takeDamageDelay = .5f;
        [SerializeField] private Transform _mouth;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private EnemySoundBehaviour _enemySound;
        private Color _originalMaterial;
        private bool _isShooting = false;
        private bool _isFrozen = false;
        private Timer _damageTimer;
        private ICanDissolve _dissolver;


        #endregion
    }
}