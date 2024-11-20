using DG.Tweening;
using UnityEngine;

namespace Game.Runtime
{
    public class FallingPlatformBehaviour : MonoBehaviour
    {
        #region Unity API

        private void Start () 
        {
            _originPosition = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            StartFallingSequence();
        }

        #endregion


        #region Main Methods

        [ContextMenu("Test falling platform")]
        private void StartFallingSequence()
        {
            _fallingSequence = DOTween.Sequence();

            _fallingSequence.Append(transform.DOShakePosition(_shakingDuration, _shakingStrength, _shakingVibrations, _shankingRandomness));
            _fallingSequence.Append(transform.DOMoveY(_fallingPositionY, _fallingDuration));
            //_fallingSequence.Append(HideOrShowPlatform(_HideMaterialAplha, _durationOfFadeOut));
            _fallingSequence.AppendInterval(_DelayForRespawn);
            _fallingSequence.AppendCallback(RespawnAfterAWhile);
        }


        private Tween HideOrShowPlatform(float fadeColor, float fadeTime)
        {
            return _meshRenderer.material.DOFade(fadeColor, fadeTime);
        }

        private void RespawnAfterAWhile()
        {
            transform.position = _originPosition;
            //_meshRenderer.material.DOFade(_ShowMaterialAlpha, _durationOfFadeIn);
        }
        #endregion


        #region Utils

        #endregion


        #region Privates & Protected

        [Header("Platform Shake")]
        [SerializeField] private float _shakingDuration = 0.5f;
        [Tooltip("force de la secousse, entre 0 et 1")]
        [SerializeField] private Vector3 _shakingStrength = new Vector3 (0.1f, 0, 0);
        [Tooltip("nombre de vibrations")]
        [SerializeField] private int _shakingVibrations = 10;
        [Tooltip("imprévisibilité de la secousse")]
        [SerializeField] private float _shankingRandomness = 10;

        [Header("Platform fall")]
        [SerializeField] private float _fallingPositionY = -15f;
        [SerializeField] private float _fallingDuration = 1f;

        [Header("Platform Disappear")]
        private float _HideMaterialAplha = 0.5f;
        private float _ShowMaterialAlpha = 1f;
        [SerializeField]
        private float _durationOfFadeOut = .5f;
        [SerializeField]
        private float _durationOfFadeIn = .1f;
        [SerializeField]
        private float _DelayForRespawn = 2f;

        [SerializeField] private MeshRenderer _meshRenderer;
        private Sequence _fallingSequence;
        private Vector3 _originPosition;


        #endregion
    }
}