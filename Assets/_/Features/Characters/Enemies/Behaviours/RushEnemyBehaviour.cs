using Data.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies.Runtime
{
    public class RushEnemyBehaviour : EnemyBaseBehaviour
    {

        #region Unity API

        void Start()
    	{
            _rigidbody = GetComponent<Rigidbody>();
            _healthBar.maxValue = m_lifePoints;
            _healthBar.value = m_lifePoints;
            _dissolver = GetComponent<ICanDissolve>();
            _attackTimer = CreateAndSubscribeTimer(m_attackDelay, ResetCoolDown);
            _damageTimer = CreateAndSubscribeTimer(_takeDamageDelay, ResumeAfterDamage);
            ResetAnimations();
        }

        void Update()
    	{
            if(!_isRushing && !_isCoolingDownAfterRush)
            {
                ResetAnimations();
            }

            Vector3 playerPosition = m_blackboard.GetValue<Vector3>("Position");
            Vector3 distanceWithPlayer = playerPosition - transform.position;
            float sqrDistance = Vector3.SqrMagnitude(distanceWithPlayer);

            var playerFollow = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);

            float detectionRange = m_maxDetectionRange * m_maxDetectionRange;
            float rageRange = _rushDistance * _rushDistance;

            if (sqrDistance < detectionRange && !_isRushing && !_isCoolingDownAfterRush)
            {
                transform.LookAt(playerFollow);

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
            UpdateTimers();
        }

        private void ResetAnimations()
        {
            m_animator.SetBool("isAttacking", false);
            m_animator.SetBool("isRaging", false);
        }

        private void OnTriggerEnter(Collider other)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            TouchedPlayer();
            if (_isCoolingDownAfterRush) return;
            if (other.gameObject.TryGetComponent(out ICanBeHurt hurt))
            {
                hurt.TakeDamage(m_damages);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            TouchedPlayer();
        }

        #endregion


        #region Main Methods

        public override void Attack()
        {
            _rushStartPosition = transform.position;
            _isRushing = true;
        }

        public override void OnLock()
        {
            _LockIndicator.SetActive(true);
        }

        public override void OnUnlock()
        {
            _LockIndicator.SetActive(false);
        }

        [ContextMenu("test dissolve")]
        private void TestDissolve()
        {
            TakeDamage(1);
        }

        public override void TakeDamage(int damage)
        {
            m_lifePoints -= damage;
            _healthBar.value = m_lifePoints;
            Recoil();
            if (m_lifePoints <= 0)
            {
                //_meshRenderer.enabled = false;
                //_particleSystem.Play();
                //Sound
            }
        }

        #endregion


        #region Utils

        private void Rush()
        {
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = Vector3.zero;

            Vector3 rushForce = transform.forward * _rushSpeed; // Augmenter la force pour une meilleure collision
            _rigidbody.AddForce(rushForce, ForceMode.VelocityChange);

            Vector3 clampedVelocity = _rigidbody.velocity;
            clampedVelocity.y = 0;
            _rigidbody.velocity = clampedVelocity;

            if (Vector3.Distance(_rushStartPosition, transform.position) >= _rushDistance - 1)
            {
                TouchedPlayer();
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.isKinematic = true;
            }
        }

        private void TouchedPlayer()
        {
            _isRushing = false;
            _isCoolingDownAfterRush = true;
            m_animator.SetBool("isRaging", false);
            m_animator.SetBool("isAttacking", true);
            SetOrResetTimer(_attackTimer);
        }

        private void Recoil()
        {
            _isCoolingDownAfterRush = true;
            SetOrResetTimer(_damageTimer);
            transform.position += -transform.forward * _recoilDistance;
        }

        private void ResetCoolDown()
        {
            m_animator.SetBool("isAttacking", false);
            _isCoolingDownAfterRush = false;
        }


        private void ResumeAfterDamage()
        {
            m_animator.SetBool("isAttacking", false);
            _isCoolingDownAfterRush = false;
            if (m_lifePoints <= 0) _dissolver.StartDissolve();
            //_meshRenderer.material.color = _originalMaterial;
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

        private bool _isRushing = false;
        private bool _isCoolingDownAfterRush = false;
        private Vector3 _rushStartPosition;

        [Header("Damages")]
        [SerializeField] private float _recoilDistance = .5f;
        [SerializeField] private float _takeDamageDelay = .5f;
        [SerializeField] private Slider _healthBar;

        [Header("References")]
         private Rigidbody _rigidbody;
        //[SerializeField] private MeshRenderer _meshRenderer;
        //[SerializeField] private ParticleSystem _particleSystem;
        //[SerializeField] private AudioClip _damagedAudio;

        private ICanDissolve _dissolver;
        private Timer _damageTimer;

        #endregion
    }
}