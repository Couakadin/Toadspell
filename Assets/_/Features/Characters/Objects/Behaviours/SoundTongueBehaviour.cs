using Data.Runtime;
using UnityEngine;

namespace Objects.Runtime
{
    public class SoundTongueBehaviour : MonoBehaviour, IHaveSound
    {
        public void PlayAudioSource()
        {
            Debug.Log("playingAudio");
            _audioSource.Play();
        }

        public void StopAudioSource()
        {
            _audioSource.Stop();
        }

        [SerializeField] private AudioSource _audioSource;
    }
}
