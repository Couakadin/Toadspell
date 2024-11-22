using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class CollisionBehaviour : MonoBehaviour
    {
        #region Unity API

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                other.gameObject.transform.parent = _parent;
                if(_projectilImpactCollider != null) _projectilImpactCollider.enabled = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                other.gameObject.transform.parent = null;
                if (_projectilImpactCollider != null) _projectilImpactCollider.enabled = true;
            }
        }

        #endregion


        #region Privates & Protected
        [SerializeField] private Transform _parent;
        [SerializeField] private Collider _projectilImpactCollider;
        #endregion
    }
}