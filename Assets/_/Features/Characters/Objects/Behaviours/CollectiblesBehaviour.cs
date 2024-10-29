using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.Runtime;

namespace Objects.Runtime
{
    public class CollectiblesBehaviour : MonoBehaviour, IAmCollectable
    {

        #region Unity API

        private void OnEnable()
        {
            //meshRenderer.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"collect + {_pointsToSend}");
            Collect();
        }

        #endregion


        #region Main Methods

        public void Collect()
        {
            
            _onCollectionEvent.Raise(_pointsToSend);
            Disappear();
        }

        public void Disappear()
        {
            //meshRenderer.enabled = false;
            //_audioSource.PlayOneShot(_collectionSound);
            //_collectionParticles.Play();
            //if (!_audioSource.isPlaying || _collectionParticles.isPlaying) return;
            gameObject.SetActive(false);
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private int _pointsToSend;
        [SerializeField] private IntEvent _onCollectionEvent;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private ParticleSystem _collectionParticles;
        [SerializeField] private AudioClip _collectionSound;
        [SerializeField] private AudioSource _audioSource;

        #endregion
    }
}