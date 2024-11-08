using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.Runtime;

namespace Objects.Runtime
{
    public class ObstacleBehaviour : MonoBehaviour, IAmLockable, IAmElement, IAmObstacle
    {
        #region Publics

        public IAmElement.Element spell => m_element;
        public IAmElement.Element m_element;

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

        public void ReactToSpell()
        {
            gameObject.SetActive(false);
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private GameObject _locker;

        #endregion
    }
}