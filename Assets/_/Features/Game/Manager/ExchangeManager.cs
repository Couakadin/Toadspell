using Data.Runtime;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class ExchangeManager : MonoBehaviour
    {

        #region Unity API

        private void Start()
        {
            _typingSpeed = _typingSpeed / 100;
            Debug.Log(_typingSpeed);
            _typingTimer = new Timer(_typingSpeed);
            _typingTimer.OnTimerFinished += OnCharacterTyping;
            _LineOnScreenTimer = new Timer(_timeOnScreenDelay);
            _LineOnScreenTimer.OnTimerFinished += DisplayNextLines;
            LaunchFirstDialogue();
        }

        private void Update()
        {
            if (_isTyping)  _typingTimer.Tick();
            if(_LineOnScreenTimer.IsRunning()) _LineOnScreenTimer.Tick();
        }

        #endregion


        #region Main Methods

        private void LaunchFirstDialogue()
        {
            Sequence firstExchange = DOTween.Sequence();
            Dialogue currentExchange = _exchanges[_currentExchangeInStoryIndex].m_Dialogues[_currentExchangeIndex];
            firstExchange.Append(_dialoguePanel.DOFade(1, _panelFadeIn));
            firstExchange.AppendCallback(() => StartDialogue(currentExchange));
        }

        private void LaunchNewDialogueExchange() 
        {
            _dialoguePanel.DOFade(1, _panelFadeIn);
            Dialogue currentExchange = _exchanges[_currentExchangeInStoryIndex].m_Dialogues[_currentExchangeIndex];
            StartDialogue(currentExchange);
        }

        public void StartDialogue(Dialogue dialogue)
        {
            _currentDialogue = dialogue;
            _currentLineIndex = 0;

            _speakerImage.sprite = dialogue.m_image;
            _characterName.text = dialogue.m_speakerName;
            DisplayNextLines();
        }

        public void DisplayNextLines()
        {
            if (_currentLineIndex < _currentDialogue.m_lines.Count)
            {
                //_typingSpeed = _currentDialogue.m_lines[_currentLineIndex].m_screenTime;
                _currentCharacterIndex = 0;
                _linesOfDialogue.text = "";
                _isTyping = true;
                _writer = _currentDialogue.m_lines[_currentLineIndex].m_sentence;

                _typingTimer.Reset();
                _typingTimer.Begin();
            }
            else
            {
                if (_currentExchangeIndex < _exchanges[_currentExchangeInStoryIndex].m_Dialogues.Count - 1)
                {
                    Debug.Log("Next person");
                    _currentExchangeIndex++;
                    StartDialogue(_exchanges[_currentExchangeInStoryIndex].m_Dialogues[_currentExchangeIndex]);
                }
                else
                {
                    _dialoguePanel.DOFade(0, _panelFadeOut);
                    if (_exchanges[0]) _onFirstExchangeFinished.Raise();
                    Debug.Log("Dialogue Has Ended");
                }
            }
        }

        #endregion


        #region Utils

        private void OnCharacterTyping()
        {
            if(_currentCharacterIndex < _writer.Length)
            {
                _linesOfDialogue.text += _writer[_currentCharacterIndex];
                _currentCharacterIndex++;
                _typingTimer.Reset();
                _typingTimer.Begin();
            }
            else
            {
                _currentLineIndex++;
                _isTyping = false;
                _LineOnScreenTimer.Reset();
                _LineOnScreenTimer.Begin();
                //DisplayNextLines();
            }
        }

        #endregion


        #region Privates & Protected

        private Dialogue _currentDialogue;
        private int _currentExchangeIndex = 0;
        private int _currentExchangeInStoryIndex = 0;
        private int _currentLineIndex = 0;
        private int _currentCharacterIndex = 0;
        private bool _isTyping = false;

        private Timer _typingTimer;
        private Timer _LineOnScreenTimer;
        private string _writer;

        [Header("Panel Fade in/out")]
        [SerializeField] private CanvasGroup _dialoguePanel;
        [SerializeField] private float _panelFadeIn;
        [SerializeField] private float _panelFadeOut;

        [Header("Dialogues Specifics")]
        [SerializeField] private List<DialoguesExchanges> _exchanges = new List<DialoguesExchanges>();
        [SerializeField] private Image _speakerImage;
        [SerializeField] private float _typingSpeed;
        [SerializeField] private float _timeOnScreenDelay;

        [Header("Text Specifics")]
        [SerializeField] private TMP_Text _characterName;
        [SerializeField] private TMP_Text _linesOfDialogue;

        [Header("Events")]
        [SerializeField] private VoidEvent _onFirstExchangeFinished;

        #endregion
    }
}