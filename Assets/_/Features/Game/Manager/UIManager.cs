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
            //InputSystem.onDeviceChange += OnDeviceChangeAdjustUI;
            _gameInput = new GameInput();
            _dialogueActions = _gameInput.Dialogue;
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
            _maxLives = _playerBlackboard.GetValue<int>("Lives");
            _spellImage.sprite = _spellList[0];
            FirstFadeIn();

            for (int i = 0; i < _maxLives; i++)
            {
                GameObject life = Instantiate(_livesPrefab, _livesTransform);
                _livesList.Add(life);
            }

            _spellDelayFloat = _playerBlackboard.GetValue<float>("SpellDelay");
            Debug.Log(_spellDelayFloat);
            _deviceUsed = _playerBlackboard.GetValue<int>("device");
            if (_deviceUsed == 1)
            {
                _tutorialPanels = _joyStickTutorial;
            }
            else if (_deviceUsed == 0)
            {
                _tutorialPanels = _keyboardTutorial;
            }


        }

        private void Update()
        {
            if (_settingsInput.triggered)
            {
                if (_settingsPanel.activeSelf == false) ActionOpenSettingMenu();
                else ActionCloseSettingsMenu();
            }

            if (_hasToWaitForSpell) ReduceSpellImageWithTimer();
        }

        #endregion


        #region Main Methods

        public void FadeOnTeleportation()
        {
            Sequence fadeSequence = DOTween.Sequence();
            fadeSequence.Append(_teleportBlackScreen.DOFade(1, _teleportFadeInDelay));
            fadeSequence.AppendInterval(_teleportIntervalDelay);
            fadeSequence.Append(_teleportBlackScreen.DOFade(0, _teleportFadeOutDelay));
        }

        public void ActionActivateGameOverScreen()
        {
            UIActivation();
            _gameOverPanel.SetActive(true);
        }

        public void ActionReloadScene() => SceneManager.LoadScene(1);


        public void ActionLoadMainMenu() => SceneManager.LoadScene(0);

        public void ActionLoadEndingScene() => SceneManager.LoadScene(2);

        public void ActionOpenSettingMenu() 
        {
            UIActivation();
            _settingsPanel.SetActive(true);
            _settingsPanel.GetComponent<CanvasGroup>().DOFade(1, .5f);
        }

        public void ActionCloseSettingsMenu() 
        {
            UIDeactivation();
            _settingsPanel.GetComponent<CanvasGroup>().DOFade(0, .5f);
            _settingsPanel.SetActive(false);
        }

        public void ActionInGamePanelSetActive() => _inGamePanel.SetActive(true);

        public void ActionSpellDelayTimer()
        {
            _spellDelayImage.fillAmount = 1;
            _hasToWaitForSpell = true;
        }
       
        public void UpdateSpellImage(int spell) => _spellImage.sprite = _spellList[spell];

        public void ActionQuitApplication() => Application.Quit();

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

        private void FirstFadeIn()
        {
            Sequence fadeSequence = DOTween.Sequence();
            fadeSequence.AppendInterval(_spawnFadeInterval);
            fadeSequence.JoinCallback(() => _onPlayerHasSpawned.Raise());
            fadeSequence.Append(_teleportBlackScreen.DOFade(0, _spawnFadeOut));
        }

        private void UpdateTutorialIndex()
        {
           _tutorialIndex++;
            if (_tutorialIndex >= _tutorialPanels.Count)
            {
                _tutorialObject.SetActive(false);
                //InputSystem.onDeviceChange -= OnDeviceChangeAdjustUI;
            }
        } 

        //private void OnDeviceChangeAdjustUI(InputDevice device, InputDeviceChange change)
        //{
        //    if (change == InputDeviceChange.Added || change == InputDeviceChange.Enabled)
        //    {
        //        SwitchUIWithDevice(device);
        //    }
        //}

        //private void SwitchUIWithDevice(InputDevice device)
        //{
        //    if (device is Joystick || device is Gamepad)
        //    {
        //        _tutorialPanels = _joyStickTutorial;
        //    }
        //    else if (device is Keyboard || device is Mouse)
        //    {
        //        _tutorialPanels = _keyboardTutorial;
        //    }
        //}

        private void UIActivation() => _onUIActivationPanels.Raise();

        private void UIDeactivation() => _onUIDeactivationPanels.Raise();

        private void ReduceSpellImageWithTimer()
        {
            _spellDelayImage.fillAmount -= 1.0f / _spellDelayFloat * Time.deltaTime;
            if (_spellDelayFloat <= 0)
            {
                _hasToWaitForSpell = false;
            }
        }

        #endregion


        #region Privates & Protected

        private int _deviceUsed;

        [Header("References")]
        [SerializeField] private Blackboard _playerBlackboard;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _inGamePanel;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private VoidEvent _onUIActivationPanels;
        [SerializeField] private VoidEvent _onUIDeactivationPanels;

        private GameInput _gameInput;
        private GameInput.DialogueActions _dialogueActions;
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
        [SerializeField] private List<CanvasGroup> _tutorialPanels = new();
        [SerializeField] private int _tutorialIndex = 0;
        private bool _isKeyboard;

        [Space(8)]
        [Header("Checkpoint UI")]
        [SerializeField] private CanvasGroup _checkpoint;
        [SerializeField] private float _checkpointTImeOnScreen;
        [SerializeField] private float _checkpointFadeIn;
        [SerializeField] private float _checkpointFadeOut;

        [Space(8)]
        [Header("Spell Caster Delay")]
        [SerializeField] private Image _spellDelayImage;
        private float _spellDelayFloat;
        private bool _hasToWaitForSpell;

        #endregion
    }
}