using Data.Runtime;
using UnityEngine;

namespace Game.Runtime
{
    public class CheckPointsBehaviour : MonoBehaviour
    {
        #region Unity API

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
                _playerBlackboard.SetValue<Vector3>("Checkpoint", _spawnPoint.position);
            }
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Blackboard _playerBlackboard;

        #endregion
    }
}