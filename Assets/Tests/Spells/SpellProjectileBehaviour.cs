using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectileBehaviour : MonoBehaviour
{
    #region Unity API

    private void Start()
    {
        Invoke(nameof(DeathAfterAWhile), 3);
    }

    void Update()
	{
		transform.Translate(Vector3.forward * Time.deltaTime * _speedOfProjectile);
	}

    #endregion

    #region Main Methods

    private void DeathAfterAWhile()
    {
        Destroy(gameObject);
    }

    #endregion

    #region Privates & Protected

    [SerializeField] private float _speedOfProjectile = 10f;
    #endregion
}