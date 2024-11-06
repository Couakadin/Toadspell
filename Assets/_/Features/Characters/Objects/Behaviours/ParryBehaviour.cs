using UnityEngine;

namespace Objects.Runtime
{
    public class ParryBehaviour : MonoBehaviour
    {
        #region Unity

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == _layersToParry)
                collision.gameObject.SetActive(false);
        }

        #endregion

        #region Privates

        [SerializeField]
        private LayerMask _layersToParry;

        #endregion

    }
}
