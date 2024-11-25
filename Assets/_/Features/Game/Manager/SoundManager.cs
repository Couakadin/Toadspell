using Codice.Client.Common.GameUI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Runtime
{
    public class SoundManager : MonoBehaviour
    {
        #region Publics
	    
        #endregion


        #region Unity API
		
    	void Start()
    	{
            _AudioMixer.GetFloat("MusicVolume",out _baseMusicVolume);
        }

    	void Update()
    	{
	
    	}

        #endregion


        #region Main Methods

        public void SwitchToDialogueMusic()
        {
            MusicChange(_dialogueSound);
        }

        public void SwitchToMainMusic()
        {
            MusicChange(_mainMusicSound);
        }

        #endregion


        #region Utils

        private void MusicChange(AudioClip clip)
        {
            Sequence lowerMusic = DOTween.Sequence();
            lowerMusic.Append(_AudioMixer.DOSetFloat("MusicVolume", -40f, _audioFadeIn));
            lowerMusic.AppendCallback(() => AddMusicToAudioSource(_mainMusicSound));
            lowerMusic.Append(_AudioMixer.DOSetFloat("MusicVolume", _baseMusicVolume, _audioFadeOut));
        }

        private void AddMusicToAudioSource(AudioClip audioClip)
        {
            _musicSource.clip = audioClip;
            _musicSource.Play();
        }
        #endregion


        #region Privates & Protected

        [Header("Audio Sources References")]
        [SerializeField] private AudioMixer _AudioMixer;
        [SerializeField] private AudioSource _musicSource;

        [Header("Music Audio Clips")]
        [SerializeField] private AudioClip _dialogueSound;
        [SerializeField] private AudioClip _mainMusicSound;

        [Header("Dialogue Music Settings")]
        [SerializeField] private float _audioFadeIn = 1.5f;
        [SerializeField] private float _audioFadeOut = 1.5f;

        private float _baseMusicVolume;

        #endregion
    }
}