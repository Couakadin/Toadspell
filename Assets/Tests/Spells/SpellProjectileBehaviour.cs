using Data.Runtime;
using Player.Runtime;
using UnityEngine;

public class SpellProjectileBehaviour : MonoBehaviour
{
    #region Publics

    public PowerBehaviour m_powerBehaviour;

    #endregion

    #region Unity API

    private void Start()
    {
        m_powerBehaviour = _playerBlackboard.GetValue<PowerBehaviour>("PowerBehaviour");
    }

    void FixedUpdate()
	{
		transform.Translate(Time.fixedDeltaTime * _speedOfProjectile * Vector3.forward);
	}

    private void OnEnable()
    {
        transform.position = _playerBlackboard.GetValue<Vector3>("Position");
        if (_tongueBlackboard.GetValue<GameObject>("currentLockedTarget") != null)
            transform.LookAt(_tongueBlackboard.GetValue<GameObject>("currentLockedTarget").transform.position);
        else if (_playerBlackboard.GetValue<bool>("IsAiming"))
        { 
            transform.rotation = Camera.main.transform.rotation;
        }
        else
        {
            gameObject.SetActive(false);
        }
        Invoke(nameof(DeathAfterAWhile), 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IAmInteractable interact))
        {
            m_powerBehaviour.spell = (PowerBehaviour.Spell)interact.spellType;
        }
        gameObject.SetActive(false);
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
    private Blackboard _tongueBlackboard;

    [SerializeField] 
    private float _speedOfProjectile = 10f;
    #endregion
}