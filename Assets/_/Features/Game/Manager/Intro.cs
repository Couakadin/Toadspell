using Data.Runtime;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class Intro : MonoBehaviour
    {
        #region Publics
	
        #endregion


        #region Unity API
		
    	void Start()
    	{
	
    	}

    	void Update()
    	{
	
    	}

        #endregion


        #region Main Methods

        [ContextMenu("test Intro")]
        public void ShowIntro()
        {
            for (int i = 0; i < _introDialogues.Count; i++)
            {
                _textIndex = 0;
                _intro = DOTween.Sequence();
                _introImage.sprite = _introDialogues[i].m_image;
                _textDialogue = _introDialogues[i];
                _introText.text = _introDialogues[i].m_lines[_textIndex].m_sentence;

                _intro.Append(_canvasGroup.DOFade(1, _fadeInTime));
                for(int j = 0; j < _introDialogues[i].m_lines.Count; j++)
                {
                    _intro.Append(_introTextAlpha.DOFade(1, _textfadeIn));
                    _intro.AppendInterval(_introDialogues[i].m_lines[j].m_screenTime);
                    _intro.Append(_introTextAlpha.DOFade(0, _textFadeOut).OnComplete(UpdateText));
                    _intro.AppendInterval(_intervalBetweenTexts);
                }

                _intro.Append(_canvasGroup.DOFade(0, _fadeOutTime));
            }
        }

        private void UpdateText()
        {
            _textIndex++;
            _introText.text = _textDialogue.m_lines[_textIndex].m_sentence;

        }
        #endregion


        #region Utils

        #endregion


        #region Privates & Protected

        private Sequence _intro;
        private int _textIndex = 0;
        private Dialogue _textDialogue;

        [Header("Background Image Specifics")]
        [SerializeField] private float _fadeInTime;
        [SerializeField] private float _fadeOutTime;
        [SerializeField] private Image _introImage;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Text Specifics")]
        [SerializeField] private TMP_Text _introText;
        [SerializeField] private CanvasGroup _introTextAlpha;
        [SerializeField] private float _textfadeIn;
        [SerializeField] private float _textFadeOut;
        [SerializeField] private float _intervalBetweenTexts;

        [Header("Story Specifics")]
        [SerializeField] private List<Dialogue> _introDialogues;

        #endregion
    }
}