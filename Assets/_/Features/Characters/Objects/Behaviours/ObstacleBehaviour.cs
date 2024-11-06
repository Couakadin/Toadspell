using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.Runtime;

namespace Objects.Runtime
{
    public class ObstacleBehaviour : MonoBehaviour, IAmLockable, IAmElement
    {
        #region Publics

        public IAmElement.Element spell => m_element;
        public IAmElement.Element m_element;

        #endregion


        #region Unity API

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out IAmElement element))
            {
                if(element.spell == m_element)
                {
                    ReactToSpell();
                }
            }
        }

        #endregion


        #region Main Methods

        public void OnLock()
        {
            Debug.Log("locked");
            _locker.SetActive(true);
        }

        public void OnUnlock()
        {
            _locker.SetActive(false);
        }

        #endregion


        #region Utils

        private void ReactToSpell()
        {
            DestroySelf();
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private GameObject _locker;

        #endregion
    }
}