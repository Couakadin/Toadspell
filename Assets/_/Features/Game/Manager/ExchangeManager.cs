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
            _cutDialogueInput = _dialogueActions.CutDialogue;
        }

        private void OnEnable()
        {
            _dialogueActions.Enable();
        }

        private void OnDisable()
        {
            _dialogueActions.Disable();
        }

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

            if (_cutDialogueInput.triggered)
            {
                EndOfDialogue();
                ResetOnCut();
            }

            if(_LineOnScreenTimer.IsRunning()) _LineOnScreenTimer.Tick();
        }

        #endregion


        #region Main Methods

        public void LaunchFirstDialogue()
        {
            if(_currentExchangeInStoryIndex > _exchanges.Count) return;
            Sequence firstExchange = DOTween.Sequence();
            Dialogue currentExchange = _exchanges[_currentExchangeInStoryIndex].m_Dialogues[_currentExchangeIndex];
            firstExchange.AppendInterval(2);
            firstExchange.Append(_dialoguePanel.DOFade(1, _panelFadeIn));
            firstExchange.AppendCallback(() => StartDialogue(currentExchange));
        }

        //private void LaunchNewDialogueExchange() 
        //{
        //    _dialoguePanel.DOFade(1, _panelFadeIn);
        //    Dialogue currentExchange = _exchanges[_currentExchangeInStoryIndex].m_Dialogues[_currentExchangeIndex];
        //    StartDialogue(currentExchange);
        //}

        public void StartDialogue(Dialogue dialogue)
        {
            _currentDialogue = dialogue;
            _currentLineIndex = 0;

            _characterBackground.sprite = dialogue.m_background;
            _speakerImage.sprite = dialogue.m_image;
            _speakerName.sprite = dialogue.m_speakerNameImage;

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

                _typingTimer.Reset();
                _typingTimer.Begin();
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
            _currentExchangeIndex = 0;
            _currentCharacterIndex = 0;
            _currentLineIndex = 0;

        }
        private void EndOfDialogue()
        {
            _dialoguePanel.DOFade(0, _panelFadeOut).OnComplete(() =>
            {
                _currentExchangeInStoryIndex++;
                if (_exchanges[0]) _onFirstExchangeFinished.Raise();
            });
        }

        private void OnCharacterTyping()
        {
            if(_currentCharacterIndex < _writer.Length)
            {
                _linesOfDialogue.text += $"{_writer[_currentCharacterIndex]}";
                _currentCharacterIndex++;
                _typingTimer.Reset();
                _typingTimer.Begin();
            }
            else
            {
                _timeOnScreenDelay = _currentDialogue.m_lines[_currentLineIndex].m_screenTime;
                _currentLineIndex++;
                _isTyping = false;
                _isSkipping = false;
                _LineOnScreenTimer.UpdateTimer(_timeOnScreenDelay);
                _LineOnScreenTimer.Reset();
                _LineOnScreenTimer.Begin();
            }
        }

        private void SkipText()
        {
            _linesOfDialogue.text = $"{_writer}";
            _currentCharacterIndex = _writer.Length;
            _currentLineIndex++;

            _isTyping = false;

            _LineOnScreenTimer.Reset();
            _LineOnScreenTimer.Begin();
           
            _isSkipping=false;
        }

        #endregion


        #region Privates & Protected

        private GameInput _gameInput;
        private GameInput.DialogueActions _dialogueActions;
        private InputAction _skipInput;
        private InputAction _cutDialogueInput;
        
        private Dialogue _currentDialogue;
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

        #endregion
    }
}