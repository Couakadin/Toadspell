using Data.Runtime;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class UIManager : MonoBehaviour
    {
        #region Unity API

        private void Awake()
        {
            InputSystem.onDeviceChange += OnDeviceChangeAdjustUI;
            _gameInput = new GameInput();
            _dialogueActions = _gameInput.Dialogue;
            _gameplayActions = _gameInput.Gameplay;
            _settingsInput = _dialogueActions.Settings;
        }

        private void OnEnable()
        {
            _gameInput.Enable();
        }

        private void OnDisable()
        {
            _gameInput.Disable();
        }

        private void Start()
    	{
            _gameOverPanel.SetActive(false);
            _tutorialIndex = 0;
            _tutorialPanels = _keyboardTutorial;
            _maxLives = _playerBlackboard.GetValue<int>("Lives");
            _spellImage.sprite = _spellList[0];
            FirstFadeIn();

            for (int i = 0; i < _maxLives; i++)
            {
                GameObject life = Instantiate(_livesPrefab, _livesTransform);
                _livesList.Add(life);
            }
        }

        private void Update()
        {
            if (_settingsInput.triggered) ActionOpenSettingMenu();
        }

        #endregion


        #region Main Methods

        private void FirstFadeIn()
        {
            _onPlayerHasSpawned.Raise();
            Sequence fadeSequence = DOTween.Sequence();
            fadeSequence.AppendInterval(_spawnFadeInterval);
            fadeSequence.Append(_teleportBlackScreen.DOFade(0, _spawnFadeOut));
        }

        public void FadeOnTeleportation()
        {
            Sequence fadeSequence = DOTween.Sequence();
            fadeSequence.Append(_teleportBlackScreen.DOFade(1, _teleportFadeInDelay));
            fadeSequence.AppendInterval(_teleportIntervalDelay);
            fadeSequence.Append(_teleportBlackScreen.DOFade(0, _teleportFadeOutDelay));
        }

        [ContextMenu("GameOverScreen")]
        public void ActionActivateGameOverScreen()
        {
            UIActivation();
            _gameOverPanel.SetActive(true);
        }

        public void ActionReloadScene()
        {
            SceneManager.LoadScene(1);
        }

        public void ActionLoadMainMenu() => SceneManager.LoadScene(0);

        [ContextMenu("settings")]
        public void ActionOpenSettingMenu() 
        {
            UIActivation();
            _settingsPanel.SetActive(true);
        }

        public void ActionCloseSettingsMenu() 
        {
            UIDeactivation();
            _settingsPanel.SetActive(false);
        }

        public void ActionInGamePanelSetActive() => _inGamePanel.SetActive(true);
       
        public void UpdateSpellImage(int spell)
        {
            _spellImage.sprite = _spellList[spell];
        }

        public void UpdateLives()
        {
            _currentLives = _playerBlackboard.GetValue<int>("Lives");
            if (_currentLives < 0) 
            { 
                _currentLives = 0;
                return;
            }
            for (int i = 0; i < _livesList.Count; i++)
            {
                _livesList[i].SetActive(false);
            }
            if(_currentLives > _livesList.Count)
            {
                GameObject life = Instantiate(_livesPrefab, _livesTransform);
                _livesList.Add(life);
            }
            for(int i = 0; i < _currentLives; i++)
            {
                _livesList[i].SetActive(true);
            }
        }

        [ContextMenu("tutorial")]
        public void ShowTutorial()
        {
            if (_tutorialIndex >= _tutorialPanels.Count) return;
            Sequence tutorialSequence = DOTween.Sequence();
            tutorialSequence.Append(_tutorialPanels[_tutorialIndex].DOFade(1, _tutorialFadeIn));
            tutorialSequence.AppendInterval(_tutorialTimeOnScreen);
            tutorialSequence.Append(_tutorialPanels[_tutorialIndex].DOFade(0, _tutorialFadeOut)).OnComplete(UpdateTutorialIndex);   
        }

        public void ActionCheckpointRegistered()
        {
            Sequence checkpoint = DOTween.Sequence();
            checkpoint.Append(_checkpoint.DOFade(1, _checkpointFadeIn));
            checkpoint.AppendInterval(_checkpointTImeOnScreen);
            checkpoint.Append(_checkpoint.DOFade(0, _checkpointFadeOut));
        }

        #endregion


        #region Utils

        private void UpdateTutorialIndex()
        {
           _tutorialIndex++;
            if (_tutorialIndex >= _tutorialPanels.Count)
            {
                _tutorialObject.SetActive(false);
                InputSystem.onDeviceChange -= OnDeviceChangeAdjustUI;
            }
        } 

        private void OnDeviceChangeAdjustUI(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Added || change == InputDeviceChange.Enabled)
            {
                SwitchUIWithDevice(device);
            }
        }

        private void SwitchUIWithDevice(InputDevice device)
        {
            if (device is Gamepad)
            {
                _tutorialPanels = _joyStickTutorial;
            }
            else if (device is Keyboard || device is Mouse)
            {
                _tutorialPanels = _keyboardTutorial;
            }
        }

        private void UIActivation()
        {
            _onUIActivationPanels.Raise();
            Cursor.lockState = CursorLockMode.None;
        }

        private void UIDeactivation()
        {
            _onUIDeactivationPanels.Raise();
            Cursor.lockState = CursorLockMode.Locked;
        }

        #endregion


        #region Privates & Protected

        [Header("References")]
        [SerializeField] private Blackboard _playerBlackboard;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _inGamePanel;
        [SerializeField] private GameObject _tutorialTigger;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private VoidEvent _onUIActivationPanels;
        [SerializeField] private VoidEvent _onUIDeactivationPanels;

        private GameInput _gameInput;
        private GameInput.DialogueActions _dialogueActions;
        private GameInput.GameplayActions _gameplayActions;
        private InputAction _settingsInput;

        [Space(8)]
        [Header("On Start Fade In")]
        [SerializeField] private float _spawnFadeInterval = .5f;
        [SerializeField] private float _spawnFadeOut = 1f;
        [SerializeField] private VoidEvent _onPlayerHasSpawned;

        [Space(8)]
        [Header("Teleport Black Screen Settings")]
        [SerializeField] private CanvasGroup _teleportBlackScreen;
        [SerializeField] private float _teleportFadeInDelay = 1f;
        [SerializeField] private float _teleportIntervalDelay = .2f;
        [SerializeField] private float _teleportFadeOutDelay = 1.5f;

        [Space(8)]
        [Header("Lives")]
        [SerializeField] private GameObject _livesPrefab;
        [SerializeField] private Transform _livesTransform;
        private int _maxLives;
        private int _currentLives;
        [SerializeField] private List<GameObject> _livesList;

        [Space(8)]
        [Header("Spells")]
        [SerializeField] private List<Sprite> _spellList = new List<Sprite>();
        [SerializeField] private Image _spellImage;
        [SerializeField] private int _spell;

        [Space(8)]
        [Header("Tutorial")]
        [SerializeField] private float _tutorialTimeOnScreen;
        [SerializeField] private float _tutorialFadeIn;
        [SerializeField] private float _tutorialFadeOut;
        [SerializeField] private GameObject _tutorialObject;
        [SerializeField] private List<CanvasGroup> _keyboardTutorial;
        [SerializeField] private List<CanvasGroup> _joyStickTutorial;
        private List<CanvasGroup> _tutorialPanels = new();
        [SerializeField] private int _tutorialIndex = 0;
        private bool _isKeyboard;

        [Space(8)]
        [Header("Checkpoint UI")]
        [SerializeField] private CanvasGroup _checkpoint;
        [SerializeField] private float _checkpointTImeOnScreen;
        [SerializeField] private float _checkpointFadeIn;
        [SerializeField] private float _checkpointFadeOut;


        #endregion
    }
}