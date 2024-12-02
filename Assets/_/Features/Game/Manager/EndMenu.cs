using Data.Runtime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.Runtime
{
    public class EndMenu : MonoBehaviour
    {
        #region Unity

        private void Update()
        {
            if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame) _deviceUsed = 1;
            else if (Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame || Mouse.current != null && Mouse.current.wasUpdatedThisFrame) _deviceUsed = 0;
        }

        #endregion


        #region Methods

        public void RestartGame(int loadGameScene) => SceneManager.LoadScene(loadGameScene);
        public void QuitGame() => Application.Quit();
        public void TellMeWhichDeviceWasUsed(Blackboard playerBlackboard) => playerBlackboard.SetValue<int>("device", _deviceUsed);

        #endregion


        #region Privates

        private int _deviceUsed;

        #endregion
    }
}
