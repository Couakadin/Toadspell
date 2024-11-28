using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Runtime
{
    public class SoundManager : MonoBehaviour
    {
        #region Unity API

        private void Awake()
        {
            InitializeAudioSources();
        }

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

        public void GameOverMusic()
        {
            _gameOverSource.enabled = true;
            _mainMusicSource.enabled = false;
            _dialogueMusicSource.enabled = false;
        }

        #endregion


        #region Utils

        private void MusicChangeToMain()
        {
            Sequence lowerMusic = DOTween.Sequence();
            lowerMusic.Append(_AudioMixer.DOSetFloat("DialogueMusicVolume", -55f, _audioFadeIn));
            lowerMusic.Join(_AudioMixer.DOSetFloat("MainMusicVolume", -7.00f, _audioFadeIn));
            //lowerMusic.AppendCallback(() => AddMusicToAudioSource(_mainMusicSound));
            //lowerMusic.Append(_AudioMixer.DOSetFloat("MusicVolume", _baseMusicVolume, _audioFadeOut));
        }

        private void MusicChangeToDialogue()
        {
            Sequence lowerMusic = DOTween.Sequence();
            lowerMusic.Append(_AudioMixer.DOSetFloat("MainMusicVolume", -55f, _audioFadeIn));
            lowerMusic.Join(_AudioMixer.DOSetFloat("DialogueMusicVolume", -22f, _audioFadeIn));
        }
        
        private void InitializeAudioSources()
        {
            _gameOverSource.enabled = false;
            _mainMusicSource.enabled = true;
            _dialogueMusicSource.enabled = true;
        }

        #endregion


        #region Privates & Protected

        [Header("Audio Sources References")]
        [SerializeField] private AudioMixer _AudioMixer;
        [SerializeField] private AudioSource _dialogueMusicSource;
        [SerializeField] private AudioSource _mainMusicSource;
        [SerializeField] private AudioSource _gameOverSource;

        [Header("Dialogue Music Settings")]
        [SerializeField] private float _audioFadeIn = 1.5f;

        [Header("GameOver Settings")]
        [SerializeField] private AudioMixerSnapshot _baseSnapshot;
        [SerializeField] private AudioMixerSnapshot _paused;

        private float _baseMusicVolume;

        #endregion
    }
}