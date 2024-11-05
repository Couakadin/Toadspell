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

        private void Start()
        {
            TargetNextWayPoint();
        }

        private void FixedUpdate()
        {
            _elapsedTime += Time.deltaTime;

            float elapsedPercentage = _elapsedTime / _timeToWaypoint;
            elapsedPercentage = Mathf.SmoothStep(0,1,elapsedPercentage);
            transform.position = Vector3.Lerp(_previousWaypoint.position, _targetWayPoint.position, elapsedPercentage);

            if(elapsedPercentage >= 1)
            {
                TargetNextWayPoint();
            }
        }

        #endregion


        #region Main Methods

        public void TargetNextWayPoint()
        {
            _previousWaypoint = _waypointsList[_waypointIndex];
            _waypointIndex++;
            if (_waypointIndex >= _waypointsList.Count) _waypointIndex = 0;
            _targetWayPoint = _waypointsList[_waypointIndex];


            _elapsedTime = 0;

            float distanceToWaypoint = Vector3.Distance(_previousWaypoint.position, _targetWayPoint.position);
            _timeToWaypoint = distanceToWaypoint / _moveSpeed;
        }

        #endregion


        #region Privates & Protected

        [Header("Movement Information")]
        [SerializeField] private float _moveSpeed = 10f;


        [Header("Platforms Waypoints")]
        [SerializeField] private List<Transform> _waypointsList;
       
        private float _timeToWaypoint;
        private float _elapsedTime;

        private Transform _previousWaypoint;
        private Transform _targetWayPoint;

        private int _waypointIndex = 0;

        #endregion
    }
}