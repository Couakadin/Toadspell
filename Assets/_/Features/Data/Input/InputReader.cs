using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Data/Input")]
    public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IUIActions
    {
        #region Publics

        public event Action<Vector2> MoveEvent;

        public event Action JumpEvent;
        public event Action JumpCancelledEvent;

        public event Action PauseEvent;
        public event Action ResumeEvent;

        public event Action AimEvent;

        public event Action TongueEvent;

        public event Action LockEvent;

        #endregion

        #region Unity

        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();

                _gameInput.Gameplay.SetCallbacks(this);
                _gameInput.UI.SetCallbacks(this);

                SetGameplay();
            }
        }

        private void OnDisable()
        {
            DisableGameplay();
            DisableUI();
        }

        #endregion

        #region Methods

        public void SetGameplay()
        {
            _gameInput.Gameplay.Enable();
            _gameInput.UI.Disable();
        }

        public void SetUI()
        {
            _gameInput.Gameplay.Disable();
            _gameInput.UI.Enable();
        }

        public void DisableGameplay() => _gameInput.Gameplay.Disable();
        public void DisableUI() => _gameInput.UI.Disable();

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) JumpEvent?.Invoke();
            if (context.phase == InputActionPhase.Canceled) JumpCancelledEvent?.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context) =>
            MoveEvent?.Invoke(context.ReadValue<Vector2>());

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                PauseEvent?.Invoke();
                SetUI();
            }
        }

        public void OnResume(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                ResumeEvent?.Invoke();
                SetGameplay();
            }
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) AimEvent?.Invoke();
        }

        public void OnTongue(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) TongueEvent?.Invoke();
        }

        public void OnLock(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) LockEvent?.Invoke();
        }

        #endregion

        #region Privates

        private GameInput _gameInput;

        #endregion
    }
}
