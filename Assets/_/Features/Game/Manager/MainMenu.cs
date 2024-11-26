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
            _leaves.gameObject.SetActive(false);
            cinematique.Append(_backgroundPanel.DOFade(1, 2.5f)).OnComplete(StartIntro);
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

        [SerializeField] private int _loadGameScene;
        [SerializeField] private Intro _intro;
        [SerializeField] private GameObject _introPanel;
        [SerializeField] private CanvasGroup _backgroundPanel;
        [SerializeField] private ParticleSystem _leaves;

        #endregion
    }
}