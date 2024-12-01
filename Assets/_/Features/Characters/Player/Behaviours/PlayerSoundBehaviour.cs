using UnityEngine;

namespace Player.Runtime
{
    public class PlayerSoundBehaviour : MonoBehaviour
    {
        #region Main Methods

        public void PlayRandomFootstep()
        {
            if(_groundBehaviour.m_isGrounded == false) return;
            int footstep = Random.Range(0, _footsteps.Length);
            _movementsAudioSource.PlayOneShot(_footsteps[footstep]);
        }

        public void PlayJumpSound()
        {
            _movementsAudioSource.PlayOneShot(_jumpUp);
        }

        public void PlayLandingSound()
        {
            _movementsAudioSource.PlayOneShot(_jumpDown);
        }

        public void PlayTongueSound()
        {
            int random = Random.Range(0, _tongues.Length);
            _powerAudioSource.PlayOneShot(_tongues[random]);
        }

        public void PlaySpellSound(int type)
        {
            _powerAudioSource.PlayOneShot(_spells[type]);
        }

        public void PlayShieldSound()
        {
            _powerAudioSource.PlayOneShot(_shield);
        }


        #endregion


        #region Utils

        #endregion


        #region Privates & Protected

        [Header("Audioclips")]
        [SerializeField] private AudioClip[] _footsteps;
        [SerializeField] private AudioClip _jumpUp;
        [SerializeField] private AudioClip _jumpDown;
        [SerializeField] private AudioClip[] _spells;
        [SerializeField] private AudioClip[] _tongues;
        [SerializeField] private AudioClip _shield;
  
        [Header("References")]
        [SerializeField] private AudioSource _movementsAudioSource;
        [SerializeField] private AudioSource _powerAudioSource;
        [SerializeField] private GroundBehaviour _groundBehaviour;

        #endregion
    }
}