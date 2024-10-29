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
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                other.gameObject.transform.parent = null;
            }
        }

        #endregion


        #region Privates & Protected
        [SerializeField] private Transform _parent;
        #endregion
    }
}