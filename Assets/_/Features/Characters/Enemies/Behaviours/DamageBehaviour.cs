using Data.Runtime;
using UnityEngine;

namespace Enemies.Runtime
{
    public class DamageBehaviour : MonoBehaviour
    {
        private void Awake() => _damageTimer = new Timer(_damageInterval);

        private void OnEnable() => _damageTimer.OnTimerFinished += ResetDamageTimer;
        private void OnDisable() => _damageTimer.OnTimerFinished -= ResetDamageTimer;

        private void OnTriggerStay(Collider other)
        {
            if (((1 << other.gameObject.layer) & _layerMask) == 0 || _damageTimer.IsRunning()) return;
            
            if(other.TryGetComponent(out ICanBeHurt hurt))
            {
                //if (hurt == null) throw new System.Exception("The target has no ICanBeHurt interface!");
            
                hurt.TakeDamage(_damage);
                _damageTimer.Begin();
            }
        }

        private void Update() => _damageTimer.Tick();

        private void ResetDamageTimer() => _damageTimer.Reset();

        [Header("Damage Settings")]
        [Tooltip("Damage quantity to the target.")]
        [SerializeField]
        private int _damage;
        [Tooltip("Timer before each damages.")]
        [SerializeField]
        private float _damageInterval;
        [Tooltip("Target layer to damage.")]
        [SerializeField] 
        private LayerMask _layerMask;

        private Timer _damageTimer;
    }
}
