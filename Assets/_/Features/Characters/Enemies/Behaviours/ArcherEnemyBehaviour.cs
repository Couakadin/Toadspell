using Data.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies.Runtime
{
    public class ArcherEnemyBehaviour : EnemyBaseBehaviour
    {

        #region Unity API

        private void Awake()
        {
            //TryGetComponent(out _plantAnimator);
        }

        void Start()
    	{
            _healthBar.maxValue = m_lifePoints;
            _healthBar.value = m_lifePoints;
            _attackTimer = CreateAndSubscribeTimer(m_attackDelay, StartAttacking);
            _damageTimer = CreateAndSubscribeTimer(_takeDamageDelay, ResumeAfterDamage);
            //_originalMaterial = _meshRenderer.material.color;
        }

        void Update()
    	{
            Vector3 playerPosition = m_blackboard.GetValue<Vector3>("Position") /*+ new Vector3(0,_ShootInTheAir, 0)*/;
            var distanceWithPlayer = (playerPosition - transform.position).magnitude;

            if (distanceWithPlayer < m_maxDetectionRange)
            {
                //m_animator.SetBool("Attacking", false);
                SetOrResetTimer(_attackTimer);
                transform.LookAt(playerPosition);
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

        public override void TakeDamage(float damage)
        {
            _isShooting = false;
            m_animator.SetBool("Attacking", false);
            m_lifePoints -= damage;
            _healthBar.value = m_lifePoints;
            //_meshRenderer.material.color = Color.yellow;
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
            //_isShooting = false;
            //_meshRenderer.material.color = _originalMaterial;
            if (m_lifePoints <= 0) gameObject.SetActive(false);
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

        //[SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private float _takeDamageDelay = .5f;
        //[SerializeField] private float _ShootInTheAir = 2f;

        [SerializeField] private Slider _healthBar;
        private Color _originalMaterial;
        private bool _isShooting = false;
        private Timer _damageTimer;

        #endregion
    }
}