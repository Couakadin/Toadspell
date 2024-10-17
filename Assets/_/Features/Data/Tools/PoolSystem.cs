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
        public List<GameObject> m_poolOfObjects = new List<GameObject>();
        #endregion


        #region Unity API

        private void Awake()
        {
            for(int i = 0; i < m_poolCapacity; i++)
            {
                GameObject instance = Instantiate(m_gameObject, m_poolTransform);
                instance.SetActive(false);
                m_poolOfObjects.Add(instance);
            }
        }

        #endregion
	
	
        #region Main Methods
	
        public GameObject GetFirstAvailableObject()
        {
            for (int i = 0; i < m_poolOfObjects.Count; i++)
            {
                if(m_poolOfObjects[i].activeSelf == false) return m_poolOfObjects[i];
            }
            GameObject instance = Instantiate(m_gameObject, m_poolTransform);
            instance.SetActive(false);
            m_poolOfObjects.Add(instance);
            return instance;

        }

        #endregion
    }
}