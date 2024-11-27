using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Data.Runtime;

namespace Game.Runtime
{
    public class MainMenu : MonoBehaviour
    {
        #region Unity API

        private void Update()
        {
            if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
            {
                _deviceUsed = 1;
            }
            else if (Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame || Mouse.current != null && Mouse.current.wasUpdatedThisFrame)
            {
                _deviceUsed = 0;
            }
        }
        #endregion


        #region Main Methods

        public void LoadCinematique()
        {
            Sequence cinematique = DOTween.Sequence();
            _leaves.gameObject.SetActive(false);
            cinematique.Append(_backgroundPanel.DOFade(1, 2.5f)).OnComplete(StartIntro);
        }

        public void TellMeWhichDeviceWasUsed()
        {
            _playerBlackboard.SetValue<int>("device", _deviceUsed);
        }

        public void LoadGameScene()
        {
            SceneManager.LoadScene(_loadGameScene);
        }

        public void QuitTheGame()
        {
            Application.Quit();
        }

        private void StartIntro()
        {
            _introPanel.SetActive(true);
            _introPanel.GetComponent<CanvasGroup>().DOFade(1, 2);
        }

        #endregion


        #region Privates & Protected

        private int _deviceUsed;
        [SerializeField] private int _loadGameScene;
        [SerializeField] private Intro _intro;
        [SerializeField] private GameObject _introPanel;
        [SerializeField] private CanvasGroup _backgroundPanel;
        [SerializeField] private ParticleSystem _leaves;
        [SerializeField] private Blackboard _playerBlackboard;

        #endregion
    }
}