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
        #region Unity API
		
    	void Start()
    	{
            ShowIntro();
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

                _intro.Append(_introCanvasGroup.DOFade(1, _fadeInTime));
                for(int j = 0; j < _introDialogues[i].m_lines.Count; j++)
                {
                    _intro.Append(_introTextAlpha.DOFade(1, _textfadeIn));
                    _intro.AppendInterval(_introDialogues[i].m_lines[j].m_screenTime);
                    _intro.Append(_introTextAlpha.DOFade(0, _textFadeOut).OnComplete(UpdateText));
                    _intro.AppendInterval(_intervalBetweenTexts);
                }

                _intro.Append(_introCanvasGroup.DOFade(0, _fadeOutTime));
                _intro.Append(_backgroundCanvasGroup.DOFade(0, _backgroundFadeOut));
            }
        }

        private void UpdateText()
        {
            _textIndex++;
            _introText.text = _textDialogue.m_lines[_textIndex].m_sentence;
        }

        #endregion


        #region Privates & Protected

        private Sequence _intro;
        private int _textIndex = 0;
        private Dialogue _textDialogue;

        [Header("Background Image Specifics")]
        [SerializeField] private float _fadeInTime;
        [SerializeField] private float _fadeOutTime;
        [SerializeField] private float _backgroundFadeOut;
        [SerializeField] private Image _introImage;
        [SerializeField] private CanvasGroup _introCanvasGroup;
        [SerializeField] private CanvasGroup _backgroundCanvasGroup;

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