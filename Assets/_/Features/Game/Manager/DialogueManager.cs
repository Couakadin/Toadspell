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
            StartDialogue(_firstExchange[_currentExchangeIndex]);
        }

        void Update()
    	{

    	}

        #endregion


        #region Main Methods

        [ContextMenu("start conversation")]
        private void testDialogues() 
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

                _typingSpeed = _currentDialogue.m_lines[_currentLineIndex].m_screenTime;
                //_linesOfDialogue.text = _currentDialogue.m_lines[_currentLineIndex].m_sentence;
                _linesOfDialogue.text = _currentDialogue.m_lines[_currentLineIndex].m_sentence;


                _currentLineIndex++;
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

        #endregion


        #region Privates & Protected

        private Dialogue _currentDialogue;
        private int _currentExchangeIndex = 0;
        private int _currentLineIndex = 0;
        private int _currentCharacterIndex = 0;
        private bool _isTyping = false;

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