using Data.Runtime;
using UnityEngine;

namespace Objects.Runtime
{
    public class SpellProjectileBehaviour : MonoBehaviour
    {
        #region Unity API

        private void FixedUpdate() => transform.Translate(Time.fixedDeltaTime * _speedOfProjectile * Vector3.forward);

        private void OnEnable()
        {
            transform.position = _playerBlackboard.GetValue<Vector3>("Position");
            //transform.LookAt(_tongueBlackboard.GetValue<GameObject>("currentLockedTarget").transform.position);

            Invoke(nameof(DeathAfterAWhile), 3);
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
        private float _speedOfProjectile = 10f;
        #endregion
    }
}