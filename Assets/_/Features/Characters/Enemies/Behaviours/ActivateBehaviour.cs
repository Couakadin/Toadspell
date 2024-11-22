using UnityEngine;

namespace Enemies.Runtime
{
    public class ActivateBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _layerMask) == 0 || _script.enabled) return;
            _script.enabled = true;
            _wall.SetActive(true);
            gameObject.SetActive(false);
        }

        [Header("Activate Settings")]
        [Tooltip("Target layer that triggers activation.")]
        [SerializeField]
        private LayerMask _layerMask;
        [Tooltip("Target to activate.")]
        [SerializeField]
        private MonoBehaviour _script;
        [Tooltip("Wall to activate.")]
        [SerializeField]
        private GameObject _wall;
    }
}
