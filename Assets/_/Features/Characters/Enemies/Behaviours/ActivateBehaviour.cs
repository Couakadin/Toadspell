using Data.Runtime;
using UnityEngine;

namespace Enemies.Runtime
{
    public class ActivateBehaviour : MonoBehaviour
    {
        private void OnEnable() => _timer = new(_delay);
        private void OnDisable() => _timer?.Reset();

        private void Update() => _timer?.Tick();

        private void OnTriggerEnter(Collider other)
        {
            _timer?.Begin();
            if (_timer.IsRunning() || ((1 << other.gameObject.layer) & _layerMask) == 0 || _script.enabled) return;
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
        [Tooltip("Time before to activate.")]
        [SerializeField]
        private float _delay;

        private Timer _timer;
    }
}
