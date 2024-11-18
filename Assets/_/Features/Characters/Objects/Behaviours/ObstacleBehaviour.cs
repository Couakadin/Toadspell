using Data.Runtime;
using DG.Tweening;
using UnityEngine;

namespace Objects.Runtime
{
    public class ObstacleBehaviour : MonoBehaviour, IAmLockable, IAmElement, IAmObstacle
    {
        #region Publics

        public IAmElement.Element spell => m_element;
        public IAmElement.Element m_element;

        #endregion


        #region Unity API

        private void Start()
        {
            if (m_element == IAmElement.Element.grass) _freezeObject = GetComponent<ICanBeImmobilized>();
            _originScale = transform.localScale;
        }
        private void Update()
        {
            if (_isWatered) ResetWaterScaleEffect();
            if (_isImmobilized) ResetGroundFreezeEffect();
        }

        #endregion


        #region Main Methods

        public void OnLock()
        {
            _lockIndicator.SetActive(true);
        }

        public void OnUnlock()
        {
            _lockIndicator.SetActive(false);
        }

        public void ReactToSpell()
        {
            switch (m_element)
            {
                case IAmElement.Element.fire:
                    Debug.Log("fire");
                    FireReacion();
                    return;
                case IAmElement.Element.water:
                    Debug.Log("water");
                    WaterReaction();
                    return;
                case IAmElement.Element.arcane:
                    return;
                case IAmElement.Element.grass:
                    Debug.Log("ground");
                    GroundReaction();
                    return;
            }
        }

        #endregion


        #region Utils

        private void FireReacion()
        {
            _canDissolve = GetComponent<ICanDissolve>();
            _canDissolve.StartDissolve();
        }

        private void WaterReaction()
        {
            transform.DOScale(transform.localScale * _scaleSize, _scaleTime);
            _timer = 0;
            _isWatered = true;
        }

        private void ResetWaterScaleEffect()
        {
            _timer += Time.deltaTime;
            if (_timer >= _maxDelayToReset)
            {
                transform.DOScale(_originScale, _scaleTime);
                _isWatered = false;
            }
        }

        [ContextMenu("test ground")]
        private void GroundReaction()
        {
            _freezeObject.FreezePosition();
            _timer = 0;
            _isImmobilized = true;
        }

        private void ResetGroundFreezeEffect()
        {
            _timer += Time.deltaTime;
            if( _timer >= _maxDelayToReset)
            {
                _freezeObject.UnFreezePosition();
                _isImmobilized = false;
            }
        }

        #endregion


        #region Privates & Protected

        [Header("References for all effects")]
        [SerializeField] private GameObject _lockIndicator;
        [SerializeField] float _maxDelayToReset = 2f;
        private float _timer = 0;

        private bool _isAffectedByFire;

        [Header("Fire Effect")]
        private ICanDissolve _canDissolve;

        [Header("Water Effect")]
        [SerializeField] private float _scaleSize = 2.5f;
        [SerializeField] private float _scaleTime = .5f;
        private Vector3 _originScale;
        private bool _isWatered;

        [Header("Ground Effect")]
        private ICanBeImmobilized _freezeObject;
        private bool _isImmobilized;

        #endregion
    }
}