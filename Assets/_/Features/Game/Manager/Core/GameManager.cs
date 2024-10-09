using Data.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Manager.Runtime
{
    public class GameManager : MonoBehaviour
    {
        #region Unity

        private void OnEnable()
        {
            _inputReader.PauseEvent += HandlePause;
            _inputReader.ResumeEvent += HandleResume;
        }

        private void OnDisable()
        {
            _inputReader.PauseEvent -= HandlePause;
            _inputReader.ResumeEvent -= HandleResume;
        }

        #endregion

        #region Utils

        private void HandlePause() => _pauseMenu.SetActive(true);

        private void HandleResume() => _pauseMenu?.SetActive(false);

        #endregion

        #region Privates

        [Title("Input")]
        [SerializeField]
        private InputReader _inputReader;
        [SerializeField]
        private GameObject _pauseMenu;

        #endregion
    }
}
