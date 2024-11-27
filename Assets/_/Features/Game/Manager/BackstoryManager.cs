using Data.Runtime;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Runtime
{
    public class BackstoryManager : MonoBehaviour
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

        #endregion


        #region Utils

        private void InitializeFirstParagraph()
        {
            Sequence firstParagraph = DOTween.Sequence();
            firstParagraph.Append(_background.DOFade(1, _imgBackgroundFadeInTime));
            firstParagraph.Append(_textBackground.DOFade(1, _txtBackgroundFadeInTime)).OnComplete(() => TypeWriteText(_paragraphText));
        }

        private void TypeWriteText(TMP_Text text)
        {

        }
        #endregion


        #region Privates & Protected

        [Header("Info Shared by all")]
        [SerializeField] private float _imgBackgroundFadeInTime;
        [SerializeField] private float _txtBackgroundFadeInTime;

        [Header("Paragraph 01")]
        [SerializeField] private CanvasGroup _background;
        [SerializeField] private CanvasGroup _textBackground;
        [SerializeField] private TMP_Text _paragraphText;

        #endregion
    }
}