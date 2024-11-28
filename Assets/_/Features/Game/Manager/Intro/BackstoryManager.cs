using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Runtime
{
    public class BackstoryManager : MonoBehaviour
    {
        #region Publics

        #endregion


        #region Unity API

        void Start() 
        {

            ActivateNextBackstoryPanel();
        }

        #endregion


        #region Main Methods

        public void ActivateNextBackstoryPanel() 
        {
            if (_backstoryIndex >= _allBackstoryPanels.Count) BackstoryOverNowLoadGame();
            _allBackstoryPanels[_backstoryIndex].SetActive(true);
            _backstoryIndex++;
        }

        public void BackstoryOverNowLoadGame() 
        {
            for(int i = 0; i < _allBackstoryPanels.Count; i++)
            {
                _allBackstoryPanels[i].SetActive(false);
            }
            _mainMenu.LaunchGameScene();
        }

        #endregion


        #region Privates & Protected



        [Header("Panels")]
        [SerializeField] MainMenu _mainMenu;
        [SerializeField] private int _backstoryIndex;
        [SerializeField] private List<GameObject> _allBackstoryPanels = new();

        #endregion
    }
}