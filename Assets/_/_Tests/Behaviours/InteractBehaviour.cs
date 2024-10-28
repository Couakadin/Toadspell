using UnityEngine;

public class InteractBehaviour : MonoBehaviour, ISizeable, ILockable
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
