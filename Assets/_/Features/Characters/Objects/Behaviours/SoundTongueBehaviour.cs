using Codice.CM.Common.Tree;
using Data.Runtime;
using UnityEngine;

namespace Objects.Runtime
{
    public class SoundTongueBehaviour : MonoBehaviour, IHaveSound
    {
        private void Start()
        {
            _lastPosition = transform.position;
        }

        private void Update()
        {
            if (_canPlaySound) ObjectIsMoving();
            if(ObjectIsMoving())
            {
                Debug.Log("Moving");
                if (_audioSource.isPlaying) return;
                Debug.Log("playingAudio");
                _audioSource.Play();
            }
            if(!ObjectIsMoving() && _audioSource.isPlaying) _audioSource.Stop();
        }
        public void PlayAudioSource()
        {
            _canPlaySound = true;
            Debug.Log(_canPlaySound);
        }

        public void StopAudioSource()
        {
            _audioSource.Stop();
            _canPlaySound = false;
        }

        private bool ObjectIsMoving()
        {
            Vector3 displacement = transform.position - _lastPosition;
            _lastPosition = transform.position;
            return displacement.magnitude > 0.001;
        }

        [SerializeField] private AudioSource _audioSource;
        private Vector3 _lastPosition;
        private bool _canPlaySound;


    }
}
