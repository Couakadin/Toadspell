using Data.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class PortalPuzzleBehaviour : MonoBehaviour
    {
        #region Unity API
		
    	void Start()
    	{
            //_spawnPoints = _transformsBase.transformsList;
	        _TeleportPoint = _spawnPoints[_spawnIndexInList];
        }

    	void Update()
    	{
	
    	}

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.TryGetComponent(out ICanTeleport player))
            {
                if(_onTeleportPointVoidEvent != null) _onTeleportPointVoidEvent.Raise();
                player.Teleport(_TeleportPoint);
            }
        }

        #endregion


        #region Privates & Protected

        [Tooltip("Choisir l'index de téléportation dans la liste")]
        [SerializeField] private int _spawnIndexInList;
        //[SerializeField] private TransformListData _transformsBase;
        [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();
        private Transform _TeleportPoint;

        [Header("Teleport Event")]
        [SerializeField] private VoidEvent _onTeleportPointVoidEvent;

        #endregion
    }
}