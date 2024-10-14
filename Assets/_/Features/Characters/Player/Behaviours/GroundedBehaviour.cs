using Data.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.Runtime
{
    public class GroundedBehaviour : MonoBehaviour
    {
        #region Unity

        private void Awake()
        {
            // Blackboard Init values
            _playerBlackboard.SetValue<bool>("IsGrounded", true);
        }

        private void OnTriggerEnter(Collider other)
        {
            int otherLayer = other.gameObject.layer;

            if (otherLayer == LayerMask.NameToLayer("Ground"))
                _playerBlackboard.SetValue<bool>("IsGrounded", true);
        }

        private void OnTriggerExit(Collider other)
        {
            int otherLayer = other.gameObject.layer;

            if (otherLayer == LayerMask.NameToLayer("Ground"))
                _playerBlackboard.SetValue<bool>("IsGrounded", false);
        }

        #endregion

        #region Privates

        [Title("ScriptableObjects")]
        // Blackboard
        [SerializeField]
        private Blackboard _playerBlackboard;

        #endregion
    }
}
