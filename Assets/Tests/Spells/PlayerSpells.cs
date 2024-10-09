using Data.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{
    #region Publics
	
    #endregion


    #region Unity API
		
	void Start()
	{
		_hasASpell = false;
	}

	void Update()
	{
		if(!_hasASpell) return;
		if (_hasASpell) Debug.Log("You can cast a spell");
		if (Input.GetKeyDown(KeyCode.C))
		{
			if (_indexOfSpells == 1)
			{
				ThrowASpell(_spellBlue);
			}
			else if (_indexOfSpells == 2)
			{
				ThrowASpell(_spellRed);
			}
			else if (_indexOfSpells == 3)
			{
				ThrowASpell(_spellGreen);
			}
		}
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IAmWater water))
        {
			Debug.Log("Test spell water");
			ReactOnSpellType(1, other);
        }
        if (other.gameObject.TryGetComponent(out IAmFire fire))
        {
            Debug.Log("Test spell water");
            ReactOnSpellType(2, other);
        }
        if (other.gameObject.TryGetComponent(out IAmGrass grass))
        {
            Debug.Log("Test spell water");
            ReactOnSpellType(3, other);
        }
    }

	//  private void OnCollisionEnter(Collision collision)
	//  {
	//if (collision.gameObject.TryGetComponent(out IAmWater water))
	//{
	//	_hasASpell = true;

	//      }
	//  }

	#endregion


	#region Main Methods

	private void ReactOnSpellType(int spellType, Collider other)
	{
        _hasASpell = true;
        _indexOfSpells = spellType;
        other.gameObject.SetActive(false);
    }
	
	private void ThrowASpell(GameObject spellPrefab)
	{
        Instantiate(spellPrefab, transform);
        _spellBlue.transform.position = transform.forward;
        _spellBlue.transform.rotation = Quaternion.identity;
    }
	#endregion


	#region Utils

	#endregion


	#region Privates & Protected

	private int _indexOfSpells = 0;
    [SerializeField] private bool _hasASpell;
	[SerializeField] private GameObject _spellBlue;
	[SerializeField] private GameObject _spellRed;
	[SerializeField] private GameObject _spellGreen;
	[SerializeField] private Blackboard _blackboard;
    #endregion
}