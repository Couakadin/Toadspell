using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class SoundSettings : MonoBehaviour
    {
        #region Main Methods

        public void SetEnvironmentVolume()
        {
            SetVolume(_environmentSlider.value, _environmentSlider, "EnvironmentVolume");
        }

        public void SetVFXVolume()
        {
            SetVolume(_vfxSlider.value, _vfxSlider, "SfxVolume");
        }

        public void onPauseLowkeyIsOn()
        {
            _paused.TransitionTo(.01f);
        }

        public void onUnpausedLowkeyIsOff()
        {
            _unpaused.TransitionTo(.01f);
        }

        #endregion


        #region Utils

        private void SetVolume(float value, Slider slider, string nameOfAudio)
        {
            if (value < 1)
            {
                value = .001f;
            }
            RefreshSlider(value, slider);
            _audioMixer.SetFloat(nameOfAudio, Mathf.Log10(value / 100) * 20f);
        }

        //private void SetVolumeFromslider(Slider slider)
        //{
        //    SetVolume(slider.value, slider);
        //}

        private void RefreshSlider(float value, Slider slider)
        {
            slider.value = value;
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private Slider _environmentSlider;
        [SerializeField] private Slider _vfxSlider;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioMixerSnapshot _paused;
        [SerializeField] private AudioMixerSnapshot _unpaused;
        #endregion
    }
}