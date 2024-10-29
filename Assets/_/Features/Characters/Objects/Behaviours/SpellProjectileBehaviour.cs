using Data.Runtime;
using UnityEngine;

namespace Objects.Runtime
{
    public class SpellProjectileBehaviour : MonoBehaviour
    {
        #region Unity API

        private void OnEnable()
        {
            _target = _tongueBlackboard.GetValue<GameObject>("currentLockedTarget");

            if (_target == null) DeathAfterAWhile();
            else 
            {
                transform.position = _playerBlackboard.GetValue<Vector3>("Position");
                Invoke(nameof(DeathAfterAWhile), 10);
            }
        }

        private void OnDisable()
        {
            _target = null;
        }

        private void Update()
        {
            if (_target == null) return;

            transform.LookAt(_target.transform.position);
            _distanceToTarget = _target.transform.position - transform.position;

            transform.position += Time.deltaTime * _speedOfProjectile * _distanceToTarget.normalized;
        }

        private void OnTriggerEnter(Collider other)
        {
            DeathAfterAWhile();
        }

        #endregion

        #region Utils

        private void DeathAfterAWhile() => gameObject.SetActive(false);

        #endregion

        #region Privates & Protected

        [SerializeField]
        private Blackboard _playerBlackboard;
        [SerializeField]
        private Blackboard _tongueBlackboard;

        [SerializeField]
        private float _speedOfProjectile;

        private GameObject _target;

        private Vector3 _distanceToTarget;

        #endregion
    }
}