using Data.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies.Runtime
{
    public class ActivateBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            _timerActivate = new(_delayActivate);
            _timerDeactivate = new(_delayDeactivate);
        }

        private void OnEnable()
        {
            _timerActivate.OnTimerFinished += Activate;
            _timerDeactivate.OnTimerFinished += Deactivate;
        }
        private void OnDisable()
        {
            _timerActivate.OnTimerFinished -= Activate;
            _timerDeactivate.OnTimerFinished -= Deactivate;
        }

        private void Update()
        {
            _timerActivate?.Tick();
            _timerDeactivate?.Tick();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _layerMask) == 0 || _script.enabled) return;
            _timerActivate?.Reset();
            _timerActivate?.Begin();
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _layerMask) == 0 || !_script.enabled) return;
            _timerDeactivate?.Reset();
            _timerDeactivate?.Begin();
        }

        private void Activate()
        {
            onBossFightStarted.Raise();
            _script.enabled = true;
            _healthBar.value = _healthBar.maxValue;
            _healthBar.gameObject.SetActive(true);
            _wall.SetActive(true);
        }

        private void Deactivate()
        {
            _script.enabled = false;
            _healthBar.gameObject.SetActive(false);
            _wall.SetActive(false);
        }

        [Header("Activate Settings")]
        [Tooltip("Target layer that triggers activation.")]
        [SerializeField]
        private LayerMask _layerMask;
        [Tooltip("Target to activate.")]
        [SerializeField]
        private MonoBehaviour _script;
        [Tooltip("HealtBar to activate.")]
        [SerializeField]
        private Slider _healthBar;
        [Tooltip("Target wall to activate.")]
        [SerializeField]
        private GameObject _wall;
        [Tooltip("Time before to activate.")]
        [SerializeField]
        private float _delayActivate, _delayDeactivate;

        [SerializeField] private VoidEvent onBossFightStarted;

        private Timer _timerActivate, _timerDeactivate;
    }
}
