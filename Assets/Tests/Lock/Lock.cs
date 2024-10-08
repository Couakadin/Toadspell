using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    #region Unity API
		
	void Start()
	{
         _lockingList = new List<GameObject>();
    }

	void Update()
	{
        if (_lockingList.Count == 0) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            lockObjectInList();
        }
        _lockingList[_index].gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
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
        for(int i  = 0; i < _lockingList.Count; i++)
        {
            _lockingList[i].gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        }
        _index++;
        if(_index >= _lockingList.Count) _index = 0;
    }
    #endregion


    #region Utils

    #endregion


    #region Privates & Protected

    [SerializeField] private List<GameObject> _lockingList;
    [SerializeField] private int _index = 0;
    #endregion
}