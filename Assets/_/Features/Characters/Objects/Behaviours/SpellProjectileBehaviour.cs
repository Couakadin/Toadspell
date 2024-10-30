using Data.Runtime;
using UnityEngine;

namespace Objects.Runtime
{
    public class SpellProjectileBehaviour : MonoBehaviour
    {
        #region Unity API

        private void Awake()
        {
            _timer = new Timer(_durationOfProjectile);
        }

        private void OnEnable()
        {
            _target = _tongueBlackboard.GetValue<GameObject>("currentLockedTarget");

            if (_target == null) DeathAfterAWhile();
            else 
            {
                _timer.OnTimerFinished += DeathAfterAWhile;
                _timer.Reset();
                _timer.Begin();

                transform.position = _playerBlackboard.GetValue<Vector3>("SpellPosition");
            }
        }

        private void OnDisable()
        {
            _timer.OnTimerFinished -= DeathAfterAWhile;

            _target = null;
        }

        private void Update()
        {
            if (_target == null) return;

            _timer.Tick();

            transform.LookAt(_target.transform.position);
            _distanceToTarget = _target.transform.position - transform.position;

            transform.position += Time.deltaTime * _speedOfProjectile * _distanceToTarget.normalized;
        }

        private void OnTriggerEnter(Collider other) => DeathAfterAWhile();

        #endregion

        #region Utils

        private void DeathAfterAWhile() => gameObject.SetActive(false);

        #endregion

        #region Privates & Protected

        [Header("Blackboards")]
        [SerializeField]
        private Blackboard _playerBlackboard;
        [SerializeField]
        private Blackboard _tongueBlackboard;

        [Header("Spell Params")]
        [SerializeField]
        private float _speedOfProjectile;
        [SerializeField]
        private float _durationOfProjectile;

        private GameObject _target;

        private Vector3 _distanceToTarget;

        private Timer _timer;

        #endregion
    }
}