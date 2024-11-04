using UnityEngine;

namespace Objects.Runtime
{
    public class SpellBehaviour : MonoBehaviour
    {
        #region Unity

        private void OnTriggerEnter(Collider other) => gameObject.SetActive(false);

        #endregion
    }
}