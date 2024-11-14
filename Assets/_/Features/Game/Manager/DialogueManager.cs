using Data.Runtime;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class DialogueManager : MonoBehaviour
    {

        #region Unity API

        private void Start()
        {
            _typingTimer = new Timer(_typingSpeed);
            _typingTimer.OnTimerFinished += OnCharacterTyping;
            LaunchNewDialogueExchange();
        }

        private void Update()
        {
            if (_isTyping)
            {
                _typingTimer.Tick();
            }
        }
        #endregion


        #region Main Methods

        private void LaunchNewDialogueExchange() 
        {
            _dialoguePanel.DOFade(1, _panelFadeIn);
            StartDialogue(_firstExchange[_currentExchangeIndex]);
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
                if (_currentExchangeIndex < _firstExchange.Count -1)
                {
                    Debug.Log("Next person");
                    _currentExchangeIndex++;
                    StartDialogue(_firstExchange[_currentExchangeIndex]);
                }
                else
                {
                    _dialoguePanel.DOFade(0, _panelFadeOut);
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
                DisplayNextLines();
            }
        }

        #endregion


        #region Privates & Protected

        private Dialogue _currentDialogue;
       [SerializeField] private int _currentExchangeIndex = 0;
        [SerializeField] private int _currentLineIndex = 0;
        [SerializeField] private int _currentCharacterIndex = 0;
        [SerializeField] private bool _isTyping = false;

        private Timer _typingTimer;

        private string _writer;

        [Header("Panel Fade in/out")]
        [SerializeField] private CanvasGroup _dialoguePanel;
        [SerializeField] private float _panelFadeIn;
        [SerializeField] private float _panelFadeOut;

        [Header("Dialogues Specifics")]
        [SerializeField] private List<Dialogue> _firstExchange = new List<Dialogue>();
        [SerializeField] private Image _speakerImage;
        [SerializeField] private float _typingSpeed;

        [Header("Text Specifics")]
        [SerializeField] private TMP_Text _characterName;
        [SerializeField] private TMP_Text _linesOfDialogue;

        #endregion
    }
}