using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.Runtime;
using Player.Runtime;

namespace Objects.Runtime
{
    public class CollectiblesBehaviour : MonoBehaviour, IAmCollectable
    {

        #region Unity API

        private void OnEnable()
        {
            //meshRenderer.enabled = true;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out PowerBehaviour interact))
            {
                if (!interact.m_canEat) return;

                Collect();
                interact.m_canEat = false;
            }
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

        public void OnLock()
        {
            throw new System.NotImplementedException();
        }

        public void OnUnlock()
        {
            throw new System.NotImplementedException();
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