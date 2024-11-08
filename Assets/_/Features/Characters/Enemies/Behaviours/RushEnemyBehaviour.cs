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

            float detectionRange = m_maxDetectionRange * m_maxDetectionRange;
            float rageRange = _rushDistance * _rushDistance;

            if (sqrDistance < detectionRange && !_isRushing && !_isCoolingDownAfterRush)
            {
                transform.LookAt(playerFollow);
                // We can show here that the enemy is about to rush

                if (IsAlignedWithPlayer(playerPosition) && sqrDistance < rageRange)
                {
                    m_animator.SetBool("isRaging", true);
                    if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    {
                        Attack();
                    }
                }
            }

            if(_isRushing) Rush();

            //if (_isCoolingDownAfterRush) CooldownAfterRush();

            UpdateTimers();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isCoolingDownAfterRush) return;
            if (other.gameObject.TryGetComponent(out ICanBeHurt hurt))
            {
                //Recoil();
                m_animator.SetBool("isRaging", false);
                m_animator.SetBool("isAttacking", true);
                SetOrResetTimer(_attackTimer);
                _isRushing = false;
                _isCoolingDownAfterRush = true;
                hurt.TakeDamage(m_damages);
            }
        }

        #endregion


        #region Main Methods

        [ContextMenu("Rush")]
        public override void Attack()
        {
            //m_animator.SetBool("isRunning", true);
            _rushStartPosition = transform.position;
            _isRushing = true;
        }



        public override void OnLock()
        {
            Debug.Log("ShowYourself");
            _LockIndicator.SetActive(true);
        }

        public override void OnUnlock()
        {
            _LockIndicator.SetActive(false);
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

        #endregion


        #region Utils

        private void Rush()
        {
            transform.position += transform.forward * _rushSpeed * Time.deltaTime;
            if (Vector3.Distance(_rushStartPosition, transform.position) >= _rushDistance - 1)
            {
                Debug.Log("attack");
                m_animator.SetBool("isRaging", false);
                m_animator.SetBool("isAttacking", true);
                SetOrResetTimer(_attackTimer);
                _isCoolingDownAfterRush = true;
                _isRushing = false;
            }
        }

        //private void CooldownAfterRush()
        //{
        //   // m_animator.SetBool("isRunning", false);
        //}

        private void ResetCoolDown()
        {
            m_animator.SetBool("isAttacking", false);
            _isCoolingDownAfterRush = false;
        }

        private void Recoil()
        {
            _isCoolingDownAfterRush = true;
            SetOrResetTimer(_damageTimer);
            transform.position += -transform.forward * _recoilDistance;
        }

        [ContextMenu("Damages")]
        private void TestDamages()
        {
            TakeDamage(1);
        }

        private bool IsAlignedWithPlayer(Vector3 playerPosition)
        {
            Vector3 directionToPlayer = (playerPosition - transform.position).normalized;

            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            return angleToPlayer <= _angleDetectionRange;
        }

        private void ResumeAfterDamage()
        {
            m_animator.SetBool("isAttacking", false);
            _isCoolingDownAfterRush = false;
            if (m_lifePoints <= 0) gameObject.SetActive(false);
            //_meshRenderer.material.color = _originalMaterial;
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
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _rushDistance);
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private float _angleDetectionRange = 10f;

        [Header("Attack Specifics")]
        [SerializeField] private float _rushDistance = 5f;
        [SerializeField] private float _rushSpeed = 10f;

        private bool _isFrozen = false;
        private bool _isRushing = false;
        private bool _isCoolingDownAfterRush = false;
        private Vector3 _rushStartPosition;

        [Header("Damages")]
        [SerializeField] private float _recoilDistance = .5f;
        [SerializeField] private float _takeDamageDelay = .5f;

        [Header("References")]
        //[SerializeField] private MeshRenderer _meshRenderer;
        //[SerializeField] private ParticleSystem _particleSystem;
        //[SerializeField] private AudioClip _damagedAudio;


        private Timer _damageTimer;
        private Color _originalMaterial;

        #endregion
    }
}