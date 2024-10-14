using Data.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.Runtime
{
    public class TongueBehaviour : MonoBehaviour
    {
        #region Publics
	
        #endregion


        #region Unity API
		
    	void Start()
    	{
            _isExploring = true;
    	}

        private void OnEnable()
        {
            _inputReader.AimEvent += AimAtAnything;
            //_inputReader.TongueEvent += SendTongue;
        }

        private void OnDisable()
        {
            _inputReader.AimEvent -= AimAtAnything;
            //_inputReader.TongueEvent -= SendTongue;
        }

        void Update()
    	{
	        if (_tongue.coolDownTimer > 0) 
                _tongue.coolDownTimer -= Time.deltaTime;
        }

        #endregion


        #region Main Methods

        private void SendTongue()
        {
            if (_isAiming) StartRaycastSendingTongue();
            if (_isExploring) StartLockedSendingTongue();
        }

        private void StartRaycastSendingTongue()
        {
            if (_tongue.coolDownTimer > 0) return;

            _isSendingTongue = true;

            RaycastHit hit;
            if(Physics.Raycast(_camera.position, _camera.forward, out hit, _tongue.maxDistance, _grabLayer))
            {
                _tongueHitPoint = hit.point;
                _grabbedObject = hit.transform;

                Invoke(nameof(ExtendTongue), _tongue.grabDelay);
            }
            else
            {
                _tongueHitPoint = _camera.position + _camera.forward * _tongue.maxDistance;

                Invoke(nameof(ReturnTongue), _tongue.grabDelay);
            }
            
        }

        private void StartLockedSendingTongue()
        {
            if (_tongue.coolDownTimer > 0) return;

            _isSendingTongue = true;

            if (_lockedObject.gameObject == null)
            {
                Invoke(nameof(ReturnTongue), _tongue.grabDelay);
            }
            else
            {
                _grabbedObject = _lockedObject.gameObject.transform;
                _tongueHitPoint = _lockedObject.gameObject.transform.position;
                _tongueTip.localPosition = Vector3.MoveTowards(_tongueTip.localPosition, _grabbedObject.position, _tongue.speed * Time.deltaTime);
                Invoke(nameof(ExtendTongue), _tongue.grabDelay);
            }
        }

        private void ExtendTongue()
        {
            _grabbedObject.TryGetComponent(out _interactable);

            if (_interactable.m_grapSize == IAmInteractable.Size.Small)
            {
                // Calculer la direction entre le joueur et l'objet
                Vector3 directionToPlayer = (_player.position - _grabbedObject.position).normalized;

                // Appliquer un offset pour que l'objet se positionne devant le joueur
                Vector3 targetPosition = _player.position - directionToPlayer * _offsetDistance;

                // Mettre à jour la position de l'objet
                _grabbedObject.position = targetPosition;
            }
            else if (_interactable.m_grapSize == IAmInteractable.Size.Large)
            {
                // Calculer la direction entre le joueur et l'objet
                Vector3 directionToObject = (_tongueHitPoint - _player.position).normalized;

                // Appliquer un offset pour que le joueur se positionne devant l'objet
                Vector3 targetPosition = _tongueHitPoint - directionToObject * _offsetDistance;

                // Mettre à jour la position du joueur
                _player.position = targetPosition;
            }



            ResetCoolDownTimer();
        }
        
        private void ReturnTongue()
        {
            _tongueTip.localPosition = Vector3.MoveTowards(_tongueTip.localPosition, _initialTonguePosition, _tongue.speed * Time.deltaTime);
            ResetCoolDownTimer();
        }

        public void AimAtAnything()  // Trigger the Raycast
        {
            if (!_isAiming)
            {
                _isExploring = false;
                _isAiming = true;
            }
            else { 
                _isExploring = true;
                _isAiming = false;
            }
        }

        public void AimAtLockedTargets() // Trigger the Locked Targets
        {
            if (!_isExploring)
            {
                _isAiming = false;
                _isExploring = true;
            }
            else { 
                _isAiming = true;
                _isExploring = false;
            }
        }

        #endregion


        #region Utils

        private void ResetCoolDownTimer()
        {
            _isSendingTongue = false;
            _tongue.coolDownTimer = _tongue.coolDown;
        }

        #endregion


        #region Privates & Protected

        [Title("Input")]
        [SerializeField]
        private GameInputObject _inputReader;

        [Header("Tongue References")]
        [SerializeField] private Transform _player; // Player ref 
        [SerializeField] private Transform _camera; // Camera ref for Raycast
        [SerializeField] private Transform _tongueTip; // Tip of Tongue collider
        [SerializeField] private TongueStatsData _tongue; // Scriptable Object for the stats
        [SerializeField] private Vector3 _initialTonguePosition; //Empty Transform for return of the tongue

        [Header("Privates")]
        private Vector3 _tongueHitPoint;
        private Transform _grabbedObject;

        [Title("Layers")]
        [SerializeField]
        private LayerMask _grabLayer; // Layer of grabbing objects

        [Header("Switch Aim to Lock")]
        [SerializeField] private GameObjectData _lockedObject;
        [SerializeField] private bool _isAiming;
        [SerializeField] private bool _isExploring;
        private bool _isSendingTongue;

        [SerializeField]
        private float _offsetDistance;

        private IAmInteractable _interactable;


        #endregion
    }
}