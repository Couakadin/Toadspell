using DG.Tweening;
using UnityEngine;

namespace Game.Runtime
{
    public class MovingPlatformsBehaviour : MonoBehaviour
    {
        #region Unity API

        private void Start()
        {
            testMovement = DOTween.Sequence();
            _originPosition = transform.position;
            Move();
        }

        //private void OnCollisionStay(Collision collision)
        //{
        //    collision.transform.position = transform.position;
        //}

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
               other.gameObject.transform.parent = transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                other.gameObject.transform.parent = null;
            }
        }
        #endregion


        #region Main Methods

        public void Move()
        {
            testMovement.Append(transform.DOMove(_desiredPosition.position, _durationOfMovement).SetEase(Ease.Linear));
            testMovement.SetLoops(-1, LoopType.Yoyo);
        }

        #endregion


        #region Privates & Protected

        [Header("Movement Information")]
        [SerializeField] private Transform _desiredPosition;
        [SerializeField] private float _durationOfMovement;
        private Vector3 _originPosition;
        private Sequence testMovement;

        #endregion
    }
}