using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Runtime
{
    public class MainMenu : MonoBehaviour
    {
        #region Main Methods
	
        public void LoadCinematique()
        {
            Sequence cinematique = DOTween.Sequence();
            cinematique.Append(_menuCanvas.DOFade(0, 1)).OnComplete(StartIntro);
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
            _intro.ShowIntro();
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private int _loadGameScene;
        [SerializeField] private GameObject _introPanel;
        [SerializeField] private Intro _intro;
        [SerializeField] private CanvasGroup _menuCanvas;
        #endregion
    }
}