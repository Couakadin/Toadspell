using Data.Runtime;
using UnityEngine;

public class SpellProjectileBehaviour : MonoBehaviour
{
    #region Unity API

    private void Awake()
    {
        
    }

    private void Start()
    {
    }

    void FixedUpdate()
	{
		transform.Translate(Time.fixedDeltaTime * _speedOfProjectile * Vector3.forward);
	}

    private void OnEnable()
    {
        transform.position = _playerBlackboard.GetValue<Vector3>("Position");
        Invoke(nameof(DeathAfterAWhile), 3);
    }

    #endregion

    #region Main Methods

    private void DeathAfterAWhile()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region Privates & Protected

    [SerializeField]
    private Blackboard _playerBlackboard;

    [SerializeField] 
    private float _speedOfProjectile = 10f;
    #endregion
}