using Data.Runtime;
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


        #region Privates & Protected

        [SerializeField] private int _damages = -1;
        [SerializeField] private Blackboard _playerBlackboard;
        [SerializeField] private VoidEvent _onOutOfMapEvent;

        #endregion
    }
}