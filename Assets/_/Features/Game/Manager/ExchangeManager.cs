using Data.Runtime;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class ExchangeManager : MonoBehaviour
    {

        #region Unity API

        private void Awake()
        {
            _gameInput = new GameInput();
            _dialogueActions = _gameInput.Dialogue;
            _skipInput = _dialogueActions.Skip;
        }

        private void OnEnable() => _dialogueActions.Enable();

        private void OnDisable() => _dialogueActions.Disable();

        private void Start()
        {
            _typingSpeed = _typingSpeed / 100;

            _typingTimer = new Timer(_typingSpeed);
            _typingTimer.OnTimerFinished += OnCharacterTyping;
            _LineOnScreenTimer = new Timer(_timeOnScreenDelay);
            _LineOnScreenTimer.OnTimerFinished += DisplayNextLines;
        }

        private void Update()
        {
            if (_isTyping)
            {
                if (_skipInput.triggered && !_isSkipping)
                {
                    _isSkipping = true;
                    SkipText();
                }
                else if (!_isSkipping) _typingTimer.Tick();
            }

            if(_LineOnScreenTimer.IsRunning()) _LineOnScreenTimer.Tick();
        }

        #endregion


        #region Main Methods

        [ContextMenu("test dialogue)")]
        public void LaunchFirstDialogue()
        {
            if (_currentExchangeInStoryIndex > _exchanges.Count) return;
            Sequence firstExchange = DOTween.Sequence();
            _currentExchange = _exchanges[_currentExchangeInStoryIndex];
            if (_currentExchange.m_hasAlreadyBeenDisplayed == true)
            {
                EndOfDialogue();
                return;
            }
            Dialogue currentDialogue = _exchanges[_currentExchangeInStoryIndex].m_Dialogues[_currentExchangeIndex];
            Debug.Log(currentDialogue.ToString());
            firstExchange.AppendInterval(2);
            firstExchange.Append(_dialoguePanel.DOFade(1, _panelFadeIn));
            firstExchange.AppendCallback(() => StartDialogue(currentDialogue));
        }

        public void StartDialogue(Dialogue dialogue)
        {
            _currentDialogue = dialogue;
            _currentLineIndex = 0;

            _characterBackground.sprite = dialogue.m_background;
            _speakerImage.sprite = dialogue.m_image;
            _speakerName.sprite = dialogue.m_speakerNameImage;
            _characterPanel.DOFade(1, .5f);

            DisplayNextLines();
        }

        public void DisplayNextLines()
        {
            if (_currentLineIndex < _currentDialogue.m_lines.Count)
            {
                _currentCharacterIndex = 0;
                _linesOfDialogue.text = $"";
                _isTyping = true;
                _writer = _currentDialogue.m_lines[_currentLineIndex].m_sentence;

                ResetTimer(_typingTimer);
            }
            else
            {
                if (_currentExchangeIndex < _exchanges[_currentExchangeInStoryIndex].m_Dialogues.Count - 1)
                {
                    _currentExchangeIndex++;
                    StartDialogue(_exchanges[_currentExchangeInStoryIndex].m_Dialogues[_currentExchangeIndex]);
                }
                else
                {
                    EndOfDialogue();
                }
            }
        }

        #endregion


        #region Utils

        private void ResetOnCut()
        {
            _typingTimer.Stop();
            _currentExchangeIndex = 0;
            _currentCharacterIndex = 0;
            _currentLineIndex = 0;

        }
        private void EndOfDialogue()
        {
            _characterPanel.DOFade(0, .5f);
            _currentExchange.m_hasAlreadyBeenDisplayed = true; 
            _dialoguePanel.DOFade(0, _panelFadeOut).OnComplete(() =>
            {
                ResetOnCut();
                if (_exchanges[_currentExchangeInStoryIndex] == _exchanges[0]) _onFirstExchangeFinished.Raise();
                if (_exchanges[_currentExchangeInStoryIndex] == _exchanges[1]) _onBridgeExchangeFinished.Raise();
                if (_exchanges[_currentExchangeInStoryIndex] == _exchanges[2])
                {
                    _onBeforeBossBattleFinished.Raise();
                }
                if (_exchanges[_currentExchangeInStoryIndex] == _exchanges[3]) _onEndingDialogueFinished.Raise();
                _currentExchangeInStoryIndex++;
            });
        }

        private void OnCharacterTyping()
        {
            if(_currentCharacterIndex < _writer.Length)
            {
                _linesOfDialogue.text += $"{_writer[_currentCharacterIndex]}";
                _currentCharacterIndex++;
                ResetTimer(_typingTimer);
            }
            else
            {
                _timeOnScreenDelay = _currentDialogue.m_lines[_currentLineIndex].m_screenTime;
                _currentLineIndex++;
                _isTyping = false;
                _isSkipping = false;
                _LineOnScreenTimer.UpdateTimer(_timeOnScreenDelay);
                ResetTimer(_LineOnScreenTimer);
            }
        }

        private void SkipText()
        {
            _linesOfDialogue.text = $"{_writer}";
            _currentCharacterIndex = _writer.Length;
            _currentLineIndex++;

            _isTyping = false;

            ResetTimer(_LineOnScreenTimer);
           
            _isSkipping=false;
        }

        private void ResetTimer(Timer timer)
        {
            timer.Reset();
            timer.Begin();
        }

        #endregion


        #region Privates & Protected

        private GameInput _gameInput;
        private GameInput.DialogueActions _dialogueActions;
        private InputAction _skipInput;
        
        private Dialogue _currentDialogue;
        private DialoguesExchanges _currentExchange;
        private int _currentExchangeIndex = 0;
        [SerializeField] private int _currentExchangeInStoryIndex = 0;
        private int _currentLineIndex = 0;
        private int _currentCharacterIndex = 0;
        private bool _isTyping = false;
        private bool _isSkipping = false;

        private Timer _typingTimer;
        private Timer _LineOnScreenTimer;
        private string _writer;

        [Header("Panel Fade in/out")]
        [SerializeField] private CanvasGroup _dialoguePanel;
        [SerializeField] private CanvasGroup _characterPanel;
        [SerializeField] private float _panelFadeIn;
        [SerializeField] private float _panelFadeOut;

        [Header("Dialogues Specifics")]
        [SerializeField] private List<DialoguesExchanges> _exchanges = new List<DialoguesExchanges>();
        [SerializeField] private Image _characterBackground;
        [SerializeField] private Image _speakerImage;
        [SerializeField] private Image _speakerName;
        [SerializeField] private float _typingSpeed;
        [SerializeField] private float _timeOnScreenDelay;

        [Header("Text Specifics")]
        //[SerializeField] private TMP_Text _characterName;
        //[SerializeField] private TMP_Text _characterNameShadow;
        [SerializeField] private TMP_Text _linesOfDialogue;

        [Header("Events")]
        [SerializeField] private VoidEvent _onFirstExchangeFinished;
        [SerializeField] private VoidEvent _onBridgeExchangeFinished;
        [SerializeField] private VoidEvent _onBeforeBossBattleFinished;
        [SerializeField] private VoidEvent _onEndingDialogueFinished;


        #endregion
    }
}