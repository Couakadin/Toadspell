using UnityEngine;
using Data.Runtime;

namespace Objects.Runtime
{
    public class InteractBehaviour : MonoBehaviour, ISizeable, IAmLockable
    {
        #region Publics

        public ISizeable.Size size => m_size;
        public ISizeable.Size m_size;

        #endregion

        #region Unity

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        #endregion

        #region Methods

        public void OnLock()
        {
            Debug.Log("Show Yourself");
            if (_lockIndicator == null) return;
            _lockIndicator.SetActive(true);
        }

        public void OnUnlock()
        {
            if (_lockIndicator == null) return;
            _lockIndicator.SetActive(false);
        }

        #endregion

        #region Privates

        private MeshRenderer _meshRenderer;
        [SerializeField] private GameObject _lockIndicator;

        #endregion
    }
}