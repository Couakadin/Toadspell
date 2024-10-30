using Data.Runtime;
using UnityEngine;

namespace Enemies.Runtime
{
    public class ArcherEnemyBehaviour : EnemyBaseBehaviour
    {
        #region Publics 

        public PoolSystem m_projectilePool;

        #endregion

        #region Unity API

        void Start()
    	{
            _attackTimer = CreateAndSubscribeTimer(m_attackDelay, Attack);
            _damageTimer = CreateAndSubscribeTimer(_takeDamageDelay, ResumeAfterDamage);

            _originalMaterial = _meshRenderer.material.color;
        }

        void Update()
    	{
            Vector3 playerPosition = m_blackboard.GetValue<Vector3>("Position") + new Vector3(0,_ShootInTheAir, 0); //Keeps track of player position
            var distanceWithPlayer = (playerPosition - transform.position).magnitude; // Distance with player

            if (distanceWithPlayer < m_maxDetectionRange && _isShooting)
            {
                transform.LookAt(playerPosition);

                SetOrResetTimer(_attackTimer);
            }

            UpdateTimers();

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_maxDetectionRange);
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
            m_lifePoints -= damage;
            _isShooting = false;
            _meshRenderer.material.color = Color.yellow;
            SetOrResetTimer(_damageTimer);
        }

        public override void Attack()
        {
            GameObject projectile = m_projectilePool.GetFirstAvailableObject();
            projectile.SetActive(true);
            projectile.transform.position = transform.position;
            projectile.transform.rotation = transform.rotation;
            Debug.Log("Attack");
        }

        #endregion


        #region Utils

        private void ResumeAfterDamage()
        {
            _isShooting = true;
            _meshRenderer.material.color = _originalMaterial;
        }

        private void UpdateTimers()
        {
            if (_attackTimer.IsRunning()) _attackTimer.Tick();
            if (_damageTimer.IsRunning()) _damageTimer.Tick();
        }

        #endregion


        #region Private & Protected

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private float _takeDamageDelay = .5f;
        [SerializeField] private float _ShootInTheAir = 2f;
        private Color _originalMaterial;
        private bool _isShooting = true;
        private Timer _damageTimer;

        #endregion
    }
}