using Data.Runtime;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class FellOutOfMapBehaviour : MonoBehaviour
    {
        #region Unity API

        private void OnTriggerEnter(Collider other)
        {
            _onOutOfMapEvent.Raise();
            other.TryGetComponent(out ICanBeHurt component);
            component.TakeDamage(_damages);
        }

        #endregion


        #region Main Methods

        #endregion


        #region Utils

        #endregion


        #region Privates & Protected

        [SerializeField] private float _damages = -1;
        [SerializeField] private Blackboard _playerBlackboard;
        [SerializeField] private VoidEvent _onOutOfMapEvent;

        #endregion
    }
}