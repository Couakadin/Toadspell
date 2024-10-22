using System.Collections.Generic;
using UnityEngine;

namespace Data.Runtime
{
    public class PoolSystem : MonoBehaviour
    {
        #region Publics

        public int m_poolCapacity;
        public Transform m_poolTransform;
        public GameObject m_gameObject;
        public List<GameObject> m_poolOfObjects = new();
        #endregion


        #region Unity API

        private void Start()
        {
            for(int i = 0; i < m_poolCapacity; i++) InstancePool(m_gameObject);
        }

        #endregion
	
        #region Main Methods
	
        public GameObject GetFirstAvailableObject()
        {
            foreach (GameObject obj in m_poolOfObjects)
            {
                if (obj.activeSelf == true) continue; 
                obj.SetActive(true);
                return obj;
            }
            return InstancePool(m_gameObject);
        }

        #endregion

        #region Utils

        private GameObject InstancePool(GameObject prefab)
        {
            GameObject instance = Instantiate(m_gameObject, m_poolTransform.position, m_poolTransform.rotation, transform);
            instance.SetActive(false);
            m_poolOfObjects.Add(instance);

            return instance;
        }

        #endregion
    }
}