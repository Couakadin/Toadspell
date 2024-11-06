using UnityEngine;

namespace Objects.Runtime
{
    public class ParryBehaviour : MonoBehaviour
    {
        #region Unity

        private void OnTriggerEnter(Collider other) => other.gameObject.SetActive(false);

        #endregion

    }
}
