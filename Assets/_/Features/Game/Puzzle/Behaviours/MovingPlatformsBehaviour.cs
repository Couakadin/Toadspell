using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Data.Runtime;
using System;

namespace Game.Runtime
{
    public class MovingPlatformsBehaviour : MonoBehaviour
    {
        #region Unity API

        private void Awake()
        {
            transform.position = _waypointsList[_waypointIndex].position;
            _waypointIndex++;
            _waitTimer = new Timer(_platformWaitDelay);
            _waitTimer.OnTimerFinished += RestartMovement;
        }

        private void Update()
        {
            _waitTimer.Tick();
        }

        private void FixedUpdate()
        {
            if (!_isMoving) MovePlatform();
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
                _isMoving = true;
                _waitTimer.Reset();
                _waitTimer.Begin();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _waypointsList[_waypointIndex].position, _moveSpeed * _durationOfMovement * Time.deltaTime);
            }
        }

        #endregion


        #region Utils

        private void RestartMovement()
        {
            _isMoving = false;
        }

        #endregion


        #region Privates & Protected

        [Header("Movement Information")]
        [SerializeField] private float _durationOfMovement;

        [Header("Platforms Waypoints")]
        [SerializeField] private List<Transform> _waypointsList;
        [SerializeField] private float _platformWaitDelay = 1f;
        [SerializeField] private float _moveSpeed = 10f;
        private int _waypointIndex = 0;
        private float _distanceToTarget;
        private bool _isMoving;
        private Timer _waitTimer;

        #endregion
    }
}