using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Runtime
{
    public class EnemySoundBehaviour : MonoBehaviour
    {
        #region Publics

        #endregion


        #region Unity API

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        #endregion


        #region Main Methods

        public void PlaySoundWhenAttacking()
        {
            _audioSource.PlayOneShot(_attackSound);
        }

        public void PlaySoundWhenDying()
        {
            _audioSource.PlayOneShot(_deathSound);
        }

        public void PlayDetectionSound()
        {
            _audioSource.PlayOneShot(_detectionSound);
        }

        #endregion


        #region Privates & Protected

        [Header("Audioclips")]
        [SerializeField] private AudioClip _detectionSound;
        [SerializeField] private AudioClip _attackSound;
        [SerializeField] private AudioClip _deathSound;

        [Header("References")]
        [SerializeField] private AudioSource _audioSource;
        #endregion
    }
}