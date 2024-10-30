using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class MovingPlatformsBehaviour : MonoBehaviour
    {
        #region Unity API

        private void Awake()
        {
            transform.position = _waypointsList[_waypointIndex].position;
            _waypointIndex++;
        }

        private void FixedUpdate()
        {
            MovePlatform();
        }

        #endregion


        #region Main Methods

        public void MovePlatform()
        {
            _distanceToTarget = Vector3.Distance(transform.position, _waypointsList[_waypointIndex].position);
            if (_distanceToTarget < .1f)
            {
                _waypointIndex++;
                if (_waypointIndex >= _waypointsList.Count) _waypointIndex = 0;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _waypointsList[_waypointIndex].position, _durationOfMovement * Time.deltaTime);
            }
        }

        #endregion

        
        #region Privates & Protected

        [Header("Movement Information")]
        [SerializeField] private float _durationOfMovement;

        [Header("Platforms Waypoints")]
        [SerializeField] private List<Transform> _waypointsList;
        private int _waypointIndex = 0;
        private float _distanceToTarget;

        #endregion
    }
}