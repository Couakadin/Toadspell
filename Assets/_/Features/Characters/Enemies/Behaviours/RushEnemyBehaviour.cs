using Data.Runtime;
using UnityEngine;

namespace Enemies.Runtime
{
    public class RushEnemyBehaviour : EnemyBaseBehaviour
    {

        #region Unity API

        void Start()
    	{
            _attackTimer = CreateAndSubscribeTimer(m_attackDelay, ResetCoolDown);
            _damageTimer = CreateAndSubscribeTimer(_takeDamageDelay, ResumeAfterDamage);
        }

        void Update()
    	{
            Vector3 playerPosition = m_blackboard.GetValue<Vector3>("Position");
            Vector3 distanceWithPlayer = playerPosition - transform.position;
            float sqrDistance = Vector3.SqrMagnitude(distanceWithPlayer);

            var playerFollow = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);

            if (sqrDistance < m_maxDetectionRange * m_maxDetectionRange && !_isRushing && !_isFrozen)
            {
                transform.LookAt(playerFollow);
                // We can show here that the enemy is about to rush
                
                if(IsAlignedWithPlayer(playerPosition) && !_isCoolingDownAfterRush)
                {
                    Attack();
                }
            }

            if(_isRushing) Rush();

            if (_isCoolingDownAfterRush) CooldownAfterRush();

            UpdateTimers();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ICanBeHurt hurt))
            {
                Recoil();
                _isRushing = false;
                _isCoolingDownAfterRush = true;
                hurt.TakeDamage(_damages);
            }
        }

        #endregion


        #region Main Methods

        [ContextMenu("Rush")]
        public override void Attack()
        {
            _rushStartPosition = transform.position;
            _isRushing = true;
        }

        private void Rush()
        {
            transform.position += transform.forward * _rushSpeed * Time.deltaTime;
            if (Vector3.Distance(_rushStartPosition, transform.position) >= _rushDistance)
            {
                _isRushing = false;
                _isCoolingDownAfterRush = true;
            }
        }

        public override void OnLock()
        {
            //need to show lock
        }

        public override void OnUnlock()
        {
            //need to hide lock
        }
        
        public override void TakeDamage(float damage)
        {
            m_lifePoints -= damage;
            Recoil();
            if (m_lifePoints <= 0)
            {
                //_meshRenderer.enabled = false;
                //_particleSystem.Play();
                //Sound

            }
            //_meshRenderer.material.color = Color.yellow;

        }

        private void Recoil()
        {
            _isFrozen = true;
            transform.position += -transform.forward * _recoilDistance;
            SetOrResetTimer(_damageTimer);
        }

        public void DieWhenFalling()
        {
            gameObject.SetActive(false);
        }

        #endregion


        #region Utils
        [ContextMenu("Damages")]
        private void TestDamages()
        {
            TakeDamage(1);
        }

        private void UpdateTimers()
        {
            if (_attackTimer.IsRunning()) _attackTimer.Tick();
            if (_damageTimer.IsRunning()) _damageTimer.Tick();
        }

        private bool IsAlignedWithPlayer(Vector3 playerPosition)
        {
            Vector3 directionToPlayer = (playerPosition - transform.position).normalized;

            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            return angleToPlayer <= _angleDetectionRange;
        }

        private void CooldownAfterRush()
        {
            SetOrResetTimer(_attackTimer);
        }

        private void ResetCoolDown()
        {
            _isCoolingDownAfterRush = false;
        }

        private void ResumeAfterDamage()
        {
            _isFrozen = false;
            if (m_lifePoints <= 0) gameObject.SetActive(false);
            //_meshRenderer.material.color = _originalMaterial;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_maxDetectionRange);
        }

        #endregion


        #region Privates & Protected

        [Header("References")]
        //[SerializeField] private MeshRenderer _meshRenderer;
        //[SerializeField] private ParticleSystem _particleSystem;
        //[SerializeField] private AudioClip _damagedAudio;

        [Header("Player Detection")]
        [SerializeField] private float _angleDetectionRange = 10f;

        [Header("Attack Specifics")]
        [SerializeField] private float _damages = 1;
        [SerializeField] private float _rushSpeed = 10f;
        [SerializeField] private float _rushDistance = 5f;

        private bool _isFrozen = false;
        private bool _isRushing = false;
        private bool _isCoolingDownAfterRush = false;
        private Vector3 _rushStartPosition;

        [Header("Damages")]
        [SerializeField] private float _recoilDistance = .5f;
        [SerializeField] private float _takeDamageDelay = .5f;

        private Timer _damageTimer;
        private Color _originalMaterial;

        #endregion
    }
}