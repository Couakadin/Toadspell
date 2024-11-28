using DG.Tweening;
using UnityEngine;

namespace Game.Runtime
{
    public class MenuBackground : MonoBehaviour
    {
        #region Unity API
		
    	void Start()
    	{
            Sequence movingBackground = DOTween.Sequence();

            movingBackground.AppendInterval(_timeOnBGDelay);
            movingBackground.Append(_darkBackground01.DOFade(0, _colorChangeDelay));
            movingBackground.Append(_colorBackground.DOFade(0, _colorChangeDelay));
            movingBackground.Append(_greenBackground.DOFade(0, _colorChangeDelay));
            movingBackground.Append(_greenBackground.DOFade(1, _colorChangeDelay));
            movingBackground.Append(_colorBackground.DOFade(1, _colorChangeDelay));
            movingBackground.Append(_darkBackground01.DOFade(1, _colorChangeDelay));

            movingBackground.SetLoops(-1, LoopType.Yoyo);
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private CanvasGroup _darkBackground01;
        [SerializeField] private CanvasGroup _colorBackground;
        [SerializeField] private CanvasGroup _greenBackground;
        [SerializeField] private CanvasGroup _darkBackground02;
        [SerializeField] private float _colorChangeDelay;
        [SerializeField] private float _timeOnBGDelay;

        #endregion
    }
}