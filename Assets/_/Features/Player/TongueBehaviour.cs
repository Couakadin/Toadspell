using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Data.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Player.Runtime
{
    public class TongueBehaviour : MonoBehaviour
    {
        #region Publics
	
        #endregion


        #region Unity API
		
    	void Start()
    	{
	
    	}

    	void Update()
    	{
	        if (_tongue.coolDownTimer > 0) 
                _tongue.coolDownTimer -= Time.deltaTime;

            if (_isAiming) StartRaycastSendingTongue();

        }

        #endregion


        #region Main Methods

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
                Invoke(nameof(ExtendTongue), _tongue.grabDelay);
            }
        }

        private void ExtendTongue()
        {
            EnemySizeInformation grabbedObjectSize = _grabbedObject.GetComponent<EnemySizeInformation>();

            if(grabbedObjectSize.m_grapSize == IAmInteractable.Size.Small)
            {
                _grabbedObject.position = _player.position;
            }
            else if(grabbedObjectSize.m_grapSize == IAmInteractable.Size.Large)
            {
                _player.position = _tongueHitPoint;
            }

            _tongueTip.localPosition = Vector3.MoveTowards(_tongueTip.localPosition, _initialTonguePosition, _tongue.speed * Time.deltaTime);
            
            ResetCoolDownTimer();
        }
        
        private void ReturnTongue()
        {
            ResetCoolDownTimer();
        }

        public void AimAtAnything()  // Trigger the Raycast
        {
            if (!_isAiming)
            {
                _isLocking = false;
                _isAiming = true;
            }
        }

        public void AimAtLockedTargets() // Trigger the Locked Targets
        {
            if(!_isLocking)
            {
                _isAiming = false;
                _isLocking = true;
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
        [SerializeField] private bool _isLocking;
        private bool _isSendingTongue;




        #endregion
    }
}