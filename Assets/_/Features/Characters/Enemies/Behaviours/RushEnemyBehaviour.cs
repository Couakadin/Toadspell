using Data.Runtime;
using UnityEngine;

namespace Enemies.Runtime
{
    public class RushEnemyBehaviour : MonoBehaviour, IAmLockable
    {
        public IAmInteractable.Size m_grapSize => _enemySize;

        #region Unity API

        void Start()
    	{
            _originPosition = transform.position;
            _returnTimer = _returnDelay;
        }

    	void Update()
    	{

            Vector3 playerPosition = _blackboard.GetValue<Vector3>("Position"); //Cherche constamment le player
            var distanceWithPlayer = (playerPosition - transform.position).magnitude; //distance avec le player

            // L'ennemi ne suit le joueur du regard que sur les côtés, pas en l'air
            var playerFollow = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);

            if (distanceWithPlayer < _maxDetectionRange && !_isRushing)
            {
                transform.LookAt(playerFollow);
                // We can show here that the enemy is about to rush
                
                if(IsAlignedWithPlayer(playerPosition) && !_isCoolingDownAfterRush)
                {
                    Attack(); // Lance l'attaque si le joueur est proche
                }
            }

            if(_isRushing) Rush();

            if (_isCoolingDownAfterRush) CooldownAfterRush();

        }

        #endregion


        #region Main Methods

        [ContextMenu("Rush")]
        private void Attack()
        {
            _isRushing = true;
            _rushStartPosition = transform.position;
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

        private bool IsAlignedWithPlayer(Vector3 playerPosition)
        {
            Vector3 directionToPlayer = (playerPosition - transform.position).normalized;

            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            return angleToPlayer <= _angleDetectionRange;
        }
        private void CooldownAfterRush()
        {
            _returnTimer -= Time.deltaTime;
            if (_returnTimer <= 0)
            {
                _isCoolingDownAfterRush = false;
                _returnTimer = _returnDelay;
            }
        }
        public void OnLock()
        {
            throw new System.NotImplementedException();
        }

        public void OnUnlock()
        {
            throw new System.NotImplementedException();
        }

        #endregion


        #region Privates & Protected

        private IAmInteractable.Size _enemySize;
        [Header("References")]
        [SerializeField] private Blackboard _blackboard;
        [SerializeField] private GameObject _lockObject;
        [Header("Player Detection")]
        [SerializeField] private float _maxDetectionRange = 20f;
        [SerializeField] private float _angleDetectionRange = 10f;
        
        [Header("Attack Specifics")] 
        [SerializeField] private float _rushDistance = 5f; // Distance du rush
        [SerializeField] private float _rushSpeed = 10f; // Vitesse du rush
        [SerializeField] private float _returnSpeed = 5f; // Vitesse de retour à la position initiale
        [SerializeField] private float _returnDelay = 5f; // Reinitialise le timer
        private float _returnTimer; // Est décrémenté en Update
        private bool _isRushing = false; // Indique si l'ennemi rush
        private bool _isCoolingDownAfterRush = false;
        private Vector3 _rushStartPosition;
        private Vector3 _originPosition; // Position de départ du rush

        #endregion
    }
}