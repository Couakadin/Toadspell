using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Runtime
{
    public class MainMenu : MonoBehaviour
    {
        #region Main Methods
	
        public void LoadGameScene()
        {
            SceneManager.LoadScene(_loadGameScene);
        }

        public void QuitTheGame()
        {
            Application.Quit();
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private int _loadGameScene;
        #endregion
    }
}