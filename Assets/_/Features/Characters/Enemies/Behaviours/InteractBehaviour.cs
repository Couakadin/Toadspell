using UnityEngine;
using Data.Runtime;

namespace Enemies.Runtime
{
    public class InteractBehaviour : MonoBehaviour, IAmInteractable
    {
        #region Publics

        public IAmInteractable.Size m_grapSize => _enemySize;

        public float m_offsetDistance => _offsetDistance;

        #endregion

        #region Privates

        [SerializeField]
        private IAmInteractable.Size _enemySize;
        [SerializeField]
        private float _offsetDistance;

        #endregion
    }
}