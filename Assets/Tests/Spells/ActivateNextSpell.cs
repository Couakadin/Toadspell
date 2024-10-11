using System.Collections.Generic;
using UnityEngine;

public class ActivateNextSpell : MonoBehaviour
{
    #region Unity API

    private void Start()
    {
        _tongueInitialPos = _tongue.transform.position;
    }

    void Update()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
            _tongue.transform.position = _tongueInitialPos;
        }
		if (Input.GetKeyUp(KeyCode.V))
		{
			
			if (_spellListCount == _spellList.Count) return;
			_spellList[_spellListCount].SetActive(true);
			_spellListCount++;
        }
		if (Input.GetKeyDown(KeyCode.Escape)) 
		{
			Application.Quit();
		}
	}

	#endregion

	#region Privates & Protected
	
	private int _spellListCount = 0;
	private Vector3 _tongueInitialPos;
	[SerializeField] private GameObject _tongue;
	[SerializeField] private List<GameObject> _spellList = new List<GameObject>();
	#endregion
}