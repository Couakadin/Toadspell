using UnityEngine;
using Data.Runtime;

namespace Enemies.Runtime
{
    public class EnemySizeInformation : MonoBehaviour, IAmInteractable
    {
        #region Publics

        public IAmInteractable.Size m_grapSize => _enemySize;

        #endregion

        #region Privates

        [SerializeField]
        private IAmInteractable.Size _enemySize;

        #endregion
    }
}