using Data.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Runtime
{
    public class EnemyProjectileBehaviour : MonoBehaviour
    {
        #region Publics
	
        #endregion


        #region Unity API

        private void OnEnable()
        {
            //_meshRenderer.enabled = true;
            Invoke(nameof(LifespanOfProjectile), _endOfLife);
        }

        void Update()
    	{
            transform.Translate(Vector3.forward * Time.deltaTime * _speedOfProjectile);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
                other.TryGetComponent<ICanBeHurt>(out ICanBeHurt hurt);
                hurt.TakeDamage(_damages);
            }
            gameObject.SetActive(false);
        }

        #endregion


        #region Main Methods

        private void LifespanOfProjectile()
        {
            gameObject.SetActive(false);
        }

        #endregion
	
	
        #region Utils
	
        #endregion
	
	
        #region Privates & Protected
	    
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private float _endOfLife;
        [SerializeField] private float _speedOfProjectile;
        [SerializeField] private int _damages;


        #endregion
    }
}