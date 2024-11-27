using Data.Runtime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

namespace Game.Runtime
{
    public class SoundManager : MonoBehaviour
    {
        #region Unity API
		
    	void Start()
    	{
            _AudioMixer.GetFloat("MusicVolume",out _baseMusicVolume);
            _baseSnapshot.TransitionTo(0.01f);
        }

        #endregion


        #region Main Methods

        public void SwitchToDialogueMusic()
        {
            MusicChangeToDialogue();
        }

        public void SwitchToMainMusic()
        {
            MusicChangeToMain();
        }

        public void onPauseLowkeyIsOn()
        {
            _paused.TransitionTo(.01f);
        }

        public void onUnpausedLowkeyIsOff()
        {
            _baseSnapshot.TransitionTo(.01f);
        }

        public void ChangePitchOnteleport()
        {
            _musicSource.DOPitch(-0.34f, .2f);
        }

        public void ResetPitchAfterTeleport()
        {
            _musicSource.DOPitch(1, .2f);
        }
        #endregion


        #region Utils

        private void MusicChangeToMain()
        {
            Sequence lowerMusic = DOTween.Sequence();
            lowerMusic.Append(_AudioMixer.DOSetFloat("DialogueMusicVolume", -40f, _audioFadeIn));
            lowerMusic.Join(_AudioMixer.DOSetFloat("MainMusicVolume", -7.00f, _audioFadeIn));
            //lowerMusic.AppendCallback(() => AddMusicToAudioSource(_mainMusicSound));
            //lowerMusic.Append(_AudioMixer.DOSetFloat("MusicVolume", _baseMusicVolume, _audioFadeOut));
        }

        private void MusicChangeToDialogue()
        {
            Sequence lowerMusic = DOTween.Sequence();
            lowerMusic.Append(_AudioMixer.DOSetFloat("MainMusicVolume", -40f, _audioFadeIn));
            lowerMusic.Join(_AudioMixer.DOSetFloat("DialogueMusicVolume", _baseMusicVolume, _audioFadeIn));
        }

        #endregion


        #region Privates & Protected

        [Header("Audio Sources References")]
        [SerializeField] private AudioMixer _AudioMixer;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _mainMusicSource;

        [Header("Dialogue Music Settings")]
        [SerializeField] private float _audioFadeIn = 1.5f;

        [Header("GameOver Settings")]
        [SerializeField] private AudioMixerSnapshot _baseSnapshot;
        [SerializeField] private AudioMixerSnapshot _paused;

        private float _baseMusicVolume;

        #endregion
    }
}