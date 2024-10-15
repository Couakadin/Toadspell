using Data.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class PortalPuzzle : MonoBehaviour
    {
        #region Unity API
		
    	void Start()
    	{
            _spawnPoints = _transformsBase.transformsList;
	        _TeleportPoint = _spawnPoints[_spawnIndexInList];
        }

    	void Update()
    	{
	
    	}

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.TryGetComponent(out ICanTeleport player))
            {
                player.Teleport(_TeleportPoint);
            }
        }

        #endregion


        #region Privates & Protected

        [Tooltip("Choisir l'index de téléportation dans la liste")]
        [SerializeField] private int _spawnIndexInList;
        [SerializeField] private TransformListData _transformsBase;
        [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();
        [SerializeField] private Transform _TeleportPoint;

        #endregion
    }
}