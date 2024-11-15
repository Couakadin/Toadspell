using Data.Runtime;
using System.Collections.Generic;
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
            _cutoutFullPropertyBlock = new MaterialPropertyBlock();
            _cutoutFullPropertyBlock.SetFloat("_Cutout", 0.0f);
            _cutoutDissolvedPropertyBlock = new MaterialPropertyBlock();

        }

        void Start()
    	{
            m_renderer.SetPropertyBlock(_cutoutFullPropertyBlock);

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
            if (_isDying) EffectsWhenDying();
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
            //_meshRenderer.material.color = Color.yellow;
            SetOrResetTimer(_damageTimer);
        }

        private void StartAttacking()
        {
            _isShooting = true;
        }

        [ContextMenu("Dissolve")]
        private void StartDissolve()
        {
            _elapsedTime = 0;
            _isDying = true;
            _timeToDissolve = 10f;
        }
        private void EffectsWhenDying()
        {

            _elapsedTime += Time.deltaTime;

            float elapsedPercentage = _elapsedTime / _timeToDissolve;
            elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);

            _cutoutDissolvedPropertyBlock.SetFloat("_Cutout", elapsedPercentage);
            m_renderer.SetPropertyBlock(_cutoutDissolvedPropertyBlock);
            if(elapsedPercentage >= 1f)
            {
                _isDying = false;
            }
            //foreach(GameObject leaves in _leaves)
            //{
            //    gameObject.SetActive(false);
            //}
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
        MaterialPropertyBlock _cutoutFullPropertyBlock;
        MaterialPropertyBlock _cutoutDissolvedPropertyBlock;

        [SerializeField] private Slider _healthBar;
        [SerializeField] private List<GameObject> _leaves = new List<GameObject>();
        private Color _originalMaterial;
        private bool _isShooting = false;
        private Timer _damageTimer;
        [SerializeField] private bool _isDying = false;
        private float _elapsedTime;
        private float _timeToDissolve;

        #endregion
    }
}