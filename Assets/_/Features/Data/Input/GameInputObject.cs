using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Data/Input")]
    public class GameInputObject : ScriptableObject, GameInputAction.IGameplayActions, GameInputAction.IUIActions
    {
        #region Publics

        // Actions
        public event Action<Vector2> ExplorationEvent;

        public event Action AimEvent;

        public event Action JumpEvent;

        public event Action LockEvent;

        public event Action TongueEvent;

        public event Action PauseEvent;
        public event Action ResumeEvent;

        public event Action LookEvent;

        #endregion

        #region Unity

        private void OnEnable()
        {
            if (_gameInputAction == null)
            {
                _gameInputAction = new();

                _gameInputAction.Gameplay.SetCallbacks(this);
                _gameInputAction.UI.SetCallbacks(this);

                SetGameplay();
            }

            // Initialize the dictionary for tracking action states
            _actionStates = new();
        }

        private void OnDisable()
        {
            _gameInputAction.UI.Disable();
            _gameInputAction.Gameplay.Disable();
        }

        #endregion

        #region Methods

        // Generic method to check if a specific action is performed
        public bool IsPerformed(string actionName)
        {
            if (_actionStates.ContainsKey(actionName))
                return _actionStates[actionName];

            return false;
        }

        public void SetGameplay()
        {
            _gameInputAction.Gameplay.Enable();
            _gameInputAction.UI.Disable();
        }

        public void SetUI()
        {
            _gameInputAction.Gameplay.Disable();
            _gameInputAction.UI.Enable();
        }

        public void OnExploration(InputAction.CallbackContext context) =>
            ExplorationEvent?.Invoke(context.ReadValue<Vector2>());

        public void OnAim(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _actionStates["Aim"] = true;
                AimEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
                _actionStates["Aim"] = false;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            // Track the state of the Jump action
            if (context.phase == InputActionPhase.Performed)
            {
                _actionStates["Jump"] = true;
                JumpEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
                _actionStates["Jump"] = false;
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _actionStates["Pause"] = true;
                PauseEvent?.Invoke();
                SetUI();
            }
            else if (context.phase == InputActionPhase.Canceled)
                _actionStates["Pause"] = false;
        }

        public void OnResume(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _actionStates["Resume"] = true;
                ResumeEvent?.Invoke();
                SetGameplay();
            }
            else if (context.phase == InputActionPhase.Canceled)
                _actionStates["Resume"] = false;
        }

        public void OnLock(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _actionStates["Lock"] = true;
                LockEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
                _actionStates["Lock"] = false;
        }

        public void OnTongue(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _actionStates["Tongue"] = true;
                TongueEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
                _actionStates["Tongue"] = false;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _actionStates["Look"] = true;
                LookEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
                _actionStates["Look"] = false;
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            
        }

        #endregion

        #region Privates

        private GameInputAction _gameInputAction;
        private Dictionary<string, bool> _actionStates;

        #endregion
    }
}
