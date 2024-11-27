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
            _audioSource = GetComponent<AudioSource>();
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
            _audioSource.Play();
            _fallingSequence = DOTween.Sequence();
            _fallingSequence.Append(transform.DOShakePosition(_shakingDuration, _shakingStrength, _shakingVibrations, _shankingRandomness));
            _fallingSequence.JoinCallback(() =>_particles.Play());// Active les particules
            _fallingSequence.Append(transform.DOMoveY(_fallingPositionY, _fallingDuration));
            _fallingSequence.AppendInterval(_DelayForRespawn);
            _fallingSequence.AppendCallback(RespawnAfterAWhile);
        }

        private void RespawnAfterAWhile()
        {
            transform.position = _originPosition;
        }
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
        [SerializeField]
        private float _DelayForRespawn = 2f;

        private Sequence _fallingSequence;
        private Vector3 _originPosition;
        [SerializeField] private ParticleSystem _particles; //particule pour le shake

        private AudioSource _audioSource;

        #endregion
    }
}