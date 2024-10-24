using Data.Runtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class DialogueManager : MonoBehaviour
    {

        #region Unity API

    	void Update()
    	{
	
    	}

        #endregion


        #region Main Methods

        public void StartDialogue(Dialogue dialogue)
        {
            _currentDialogue = dialogue;
            _currentLineIndex = 0;

            _speakerImage.sprite = dialogue.m_image;
            DisplayNextLines();
        }

        private void DisplayNextLines()
        {
            if (_currentLineIndex < _currentDialogue.m_lines.Count)
            {
                _typingSpeed = _currentDialogue.m_lines[_currentLineIndex].m_screenTime;
                _linesOfDialogue.text = _currentDialogue.m_lines[_currentLineIndex].m_sentence;
            }

        }

        #endregion


        #region Utils

        #endregion


        #region Privates & Protected

        private Dialogue _currentDialogue;
        private float _typingSpeed;
        private int _currentLineIndex;

        [Header("Dialogues Specifics")]
        [SerializeField] private List<Dialogue> _firstExchange;
        [SerializeField] private Image _speakerImage;

        [Header("Text Specifics")]
        [SerializeField] private TMP_Text _linesOfDialogue;
        [SerializeField] private TMP_Text _characterName;

        #endregion
    }
}