using UnityEngine;

namespace Player.Runtime
{
    public class PlayerSoundBehaviour : MonoBehaviour
    {
        #region Unity API
		
    	void Start()
    	{
            _audioSource = GetComponent<AudioSource>();
        }

        #endregion


        #region Main Methods

        public void PlayRandomFootstep()
        {
            if(_groundBehaviour.m_isGrounded == false) return;
            int footstep = Random.Range(0, _footsteps.Length);
            _audioSource.PlayOneShot(_footsteps[footstep]);
        }

        public void PlayJumpSound()
        {
            _audioSource.PlayOneShot(_jumpUp);
        }

        public void PlayLandingSound()
        {
            _audioSource.PlayOneShot(_jumpDown);
        }


        #endregion


        #region Utils

        #endregion


        #region Privates & Protected

        [Header("Audioclips")]
        [SerializeField] private AudioClip[] _footsteps;
        [SerializeField] private AudioClip _jumpUp;
        [SerializeField] private AudioClip _jumpDown;

        [Header("References")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private GroundBehaviour _groundBehaviour;

        #endregion
    }
}