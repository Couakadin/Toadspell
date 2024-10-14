using Data.Runtime;
using UnityEngine;

namespace Enemies.Runtime
{
    public class LockableBehaviour : MonoBehaviour, IAmLockable
    {
        #region Unity

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        #endregion

        #region Methods

        public void OnLock()
        {
            _meshRenderer.material.color = Color.red;
        }

        public void OnUnlock()
        {
            _meshRenderer.material.color = Color.grey;
        }

        #endregion

        #region Privates

        private MeshRenderer _meshRenderer;

        #endregion
    }
}
