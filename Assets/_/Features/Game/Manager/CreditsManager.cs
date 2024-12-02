using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class CreditsManager : MonoBehaviour
    {
        #region Publics

        public void ShowCreditsCanvas()
        {
            Sequence creditsAppear = DOTween.Sequence();
            creditsAppear.Append(_creditsCanvas.DOFade(1, _creditsFadeIn));
            creditsAppear.Append(_creditsInfoCanvas.DOFade(1, _infoCreditsFadeIn));
        }

        public void HideCreditsCanvas()
        {
            Sequence creditsDisappear = DOTween.Sequence();
            creditsDisappear.Append(_creditsInfoCanvas.DOFade(0, _infoCreditsFadeOut));
            creditsDisappear.Append(_creditsCanvas.DOFade(0, _creditsFadeOut));
        }

        #endregion


        #region Private & Protected

        [SerializeField] private float _creditsFadeIn;
        [SerializeField] private float _creditsFadeOut;
        [SerializeField] private float _infoCreditsFadeIn;
        [SerializeField] private float _infoCreditsFadeOut;
        [SerializeField] private CanvasGroup _creditsCanvas;
        [SerializeField] private CanvasGroup _creditsInfoCanvas;

        #endregion
    }
}
