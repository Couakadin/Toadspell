using Data.Runtime;
using UnityEngine;

namespace Game.Runtime
{
    public class CheckPointsBehaviour : MonoBehaviour
    {
        #region Unity API

        private void OnTriggerEnter(Collider other)
        {
            // Vérifie si l'objet qui entre dans le trigger est sur le layer 7
            if (other.gameObject.layer == 7)
            {
                // Met à jour la position du checkpoint dans le Blackboard
                _playerBlackboard.SetValue<Vector3>("Checkpoint", _spawnPoint.position);
                if (!AreChildrenAlreadyActivated()) 
                {
                    if(_onCheckpointTriggered != null) _onCheckpointTriggered.Raise();

                    // Active les bébés spécifiés
                    ActivateChildObjects();
                }
            }
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private Transform _spawnPoint; // Point de spawn
        [SerializeField] private Blackboard _playerBlackboard; // Blackboard pour stocker la position du checkpoint
        [SerializeField] private GameObject _child1; // Premier enfant à activer
        [SerializeField] private GameObject _child2; // Deuxième enfant à activer
        [SerializeField] private VoidEvent _onCheckpointTriggered;

        /// <summary>
        /// Active les enfants spécifiés
        /// </summary>
        private void ActivateChildObjects()
        {
            if (_child1 != null)
            {
                _child1.SetActive(true);
            }

            if (_child2 != null)
            {
                _child2.SetActive(true);
            }
        }

        private bool AreChildrenAlreadyActivated()
        {
            return _child1.activeSelf;
        }

        #endregion
    }
}