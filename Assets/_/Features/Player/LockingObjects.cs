using Data.Runtime;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Runtime
{
    public class LockingObjects : MonoBehaviour
    {
        #region Unity API

        private void OnEnable()
        {
            _inputReader.LockEvent += SwitchLockObject;
        }

        private void OnDisable()
        {
            _inputReader.LockEvent -= SwitchLockObject;
        }

        void Update()
    	{
            if (_lockingList.Count == 0)
            {
                _lockedObject.gameObject = null;
                return;
            }
            
            _lockedObject.gameObject = _lockingList[_index];
            _lockingList[_index].gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        private void SwitchLockObject()
        {
            lockObjectInList();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_lockingList.Contains(other.gameObject)) return;
            _lockingList.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_lockingList.Contains(other.gameObject))
            {
                other.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
                _lockingList.Remove(other.gameObject);
            }
        }


        #endregion


        #region Main Methods

        private void lockObjectInList()
        {
            for (int i = 0; i < _lockingList.Count; i++)
            {
                _lockingList[i].gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
            _index++;
            if (_index >= _lockingList.Count) _index = 0;
        }

        #endregion


        #region Privates & Protected

        [Title("Input")]
        [SerializeField]
        private InputReader _inputReader;

        [SerializeField] private GameObjectData _lockedObject;
        [SerializeField] private List<GameObject> _lockingList;
        [SerializeField] private int _index = 0;

        #endregion
    }
}