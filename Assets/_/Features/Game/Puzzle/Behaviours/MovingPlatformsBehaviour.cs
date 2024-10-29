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

        #endregion


        #region Main Methods

        public void Move()
        {
            testMovement.Append(transform.DOMove(_desiredPosition.position, _durationOfMovement));
            testMovement.Append(transform.DOMove(_originPosition, _durationOfMovement));

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