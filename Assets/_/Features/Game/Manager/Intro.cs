using Data.Runtime;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class Intro : MonoBehaviour
    {
        #region Unity API
		
    	void Start()
    	{
            TestWithImage();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                _intro.Kill();
                LoadMainScene();
            }
        }
        #endregion


        #region Main Methods

        [ContextMenu("test IMG")]
        private void TestWithImage()
        {
            _intro = DOTween.Sequence();
            for (int i = 0; i < _testDialogueImg.Count; i++)
            {
                Dialogue _currentDialogue = _testDialogueImg[i];
                _intro.AppendCallback(() =>
                {
                    _textIndex = 0;
                    _introImage.sprite = _currentDialogue.m_image;
                    _textImg.sprite = _currentDialogue.m_lines[_textIndex].m_image;
                });


                _intro.Append(_introCanvasGroup.DOFade(1, _fadeInTime));

                for (int j = 0; j < _currentDialogue.m_lines.Count; j++)
                {
                    int currentIndex = j;

                    _intro.AppendCallback(() =>
                    {
                        if (_textIndex < _currentDialogue.m_lines.Count)
                        {
                            _textImg.sprite = _currentDialogue.m_lines[_textIndex].m_image;
                        }
                    });

                    _intro.Append(_imgIntroTextAlpha.DOFade(1, _textfadeIn));
                    _intro.AppendInterval(_currentDialogue.m_lines[currentIndex].m_screenTime);
                    _intro.Append(_imgIntroTextAlpha.DOFade(0, _textFadeOut).OnComplete(() =>
                    {
                        _textIndex++;
                    }));

                    _intro.AppendInterval(_intervalBetweenTexts);
                }
                _intro.Append(_introCanvasGroup.DOFade(0, _fadeOutTime));
            }

            _intro.Append(_backgroundCanvasGroup.DOFade(0, _backgroundFadeOut)).OnComplete(LoadMainScene);
        }

        //[ContextMenu("test Intro")]
        //public void ShowIntro()
        //{
        //    _intro = DOTween.Sequence();

        //    for (int i = 0; i < _introDialogues.Count; i++)
        //    {
        //        Dialogue _currentDialogue = _introDialogues[i];

        //        _intro.AppendCallback(() =>
        //        {
        //            _textIndex = 0;
        //            _introImage.sprite = _currentDialogue.m_image;
        //            _introText.text = _currentDialogue.m_lines[_textIndex].m_sentence;

        //        });
            
        //        _intro.Append(_introCanvasGroup.DOFade(1, _fadeInTime));

        //        for(int j = 0; j < _currentDialogue.m_lines.Count; j++)
        //        {
        //            int currentIndex = j;

        //            _intro.AppendCallback(() =>
        //            {
        //                if (_textIndex < _currentDialogue.m_lines.Count)
        //                {
        //                    _introText.text = _currentDialogue.m_lines[_textIndex].m_sentence;
        //                }
        //            });

        //            _intro.Append(_introTextAlpha.DOFade(1, _textfadeIn));
        //            _intro.AppendInterval(_currentDialogue.m_lines[currentIndex].m_screenTime);
        //            _intro.Append(_introTextAlpha.DOFade(0, _textFadeOut).OnComplete(() =>
        //            {
        //                _textIndex++;
        //            }));

        //            _intro.AppendInterval(_intervalBetweenTexts);
        //        }

        //        _intro.Append(_introCanvasGroup.DOFade(0, _fadeOutTime));
        //    }
        //    _intro.Append(_backgroundCanvasGroup.DOFade(0, _backgroundFadeOut)).OnComplete(LoadMainScene);
        //}

        private void LoadMainScene()
        {
            SceneManager.LoadScene(1);
        }

        #endregion


        #region Privates & Protected

        private Sequence _intro;
        private int _textIndex = 0;
        //private Dialogue _currentDialogue;

        [Header("Background Image Specifics")]
        [SerializeField] private float _fadeInTime;
        [SerializeField] private float _fadeOutTime;
        [SerializeField] private float _backgroundFadeOut;
        [SerializeField] private Image _introImage;
        [SerializeField] private CanvasGroup _introCanvasGroup;
        [SerializeField] private CanvasGroup _backgroundCanvasGroup;

        [Header("Text Specifics")]
        //[SerializeField] private TMP_Text _introText;
        //[SerializeField] private CanvasGroup _introTextAlpha;
        [SerializeField] private float _textfadeIn;
        [SerializeField] private float _textFadeOut;
        [SerializeField] private float _intervalBetweenTexts;

        [Header("Story Specifics")]
        //[SerializeField] private List<Dialogue> _introDialogues;

        [SerializeField] private List<Dialogue> _testDialogueImg;
        [SerializeField] private Image _textImg;
        [SerializeField] private CanvasGroup _imgIntroTextAlpha;

        #endregion
    }
}