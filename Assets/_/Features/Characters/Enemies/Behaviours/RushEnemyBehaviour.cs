using Data.Runtime;
using Meryel.UnityCodeAssist.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using System;

namespace Enemies.Runtime
{
    public class RushEnemyBehaviour : EnemyBaseBehaviour
    {

        #region Unity API

        void Start()
    	{
            _attackTimer = CreateAndSubscribeTimer(m_attackDelay, ResetCoolDown);
            _damageTimer = CreateAndSubscribeTimer(_takeDamageDelay, ResumeAfterDamage);
            _originalMaterial = _meshRenderer.material.color;
        }

        void Update()
    	{

            Vector3 playerPosition = _targetToReplaceWithPlayer.transform.position; //m_blackboard.GetValue<Vector3>("Position"); //Keeps track of player
            var distanceWithPlayer = (playerPosition - transform.position).magnitude; //Distance with player

            // Only follows player on the sides, not up
            var playerFollow = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);

            if (distanceWithPlayer < m_maxDetectionRange && !_isRushing && !_isFrozen)
            {
                transform.LookAt(playerFollow);
                // We can show here that the enemy is about to rush
                
                if(IsAlignedWithPlayer(playerPosition) && !_isCoolingDownAfterRush)
                {
                    Attack(); // Attacks if player is aligned
                }
            }

            if(_isRushing) Rush();

            if (_isCoolingDownAfterRush) CooldownAfterRush();

            UpdateTimers();
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
            throw new System.NotImplementedException();
        }

        public override void OnUnlock()
        {
            throw new System.NotImplementedException();
        }

        
        public override void TakeDamage(float damage)
        {
            m_lifePoints -= damage;
            _isFrozen = true;
            transform.position += -transform.forward * _recoilDistance;
            _meshRenderer.material.color = Color.yellow;
            SetOrResetTimer(_damageTimer);
        }

        [ContextMenu("Damages")]
        private void TestDamages()
        {
            TakeDamage(1);
        }

        #endregion


        #region Utils
        
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
            _meshRenderer.material.color = _originalMaterial;
        }

        #endregion


        #region Privates & Protected

        [Header("References")]
        [SerializeField] private GameObject _targetToReplaceWithPlayer;
        [SerializeField] private MeshRenderer _meshRenderer;

        [Header("Player Detection")]
        [SerializeField] private float _angleDetectionRange = 10f;
        
        [Header("Attack Specifics")] 
        [SerializeField] private float _rushDistance = 5f;
        [SerializeField] private float _rushSpeed = 10f;
        private bool _isFrozen = false;
        private bool _isRushing = false; // Indicates if attacking
        private bool _isCoolingDownAfterRush = false; //Indicates if cooldown is playing
        private Vector3 _rushStartPosition;

        [Header("Damages")]
        [SerializeField] private float _recoilDistance = .5f;
        [SerializeField] private float _takeDamageDelay = .5f;
        private Timer _damageTimer;
        private Color _originalMaterial;

        #endregion
    }
}