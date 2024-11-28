using Data.Runtime;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class BackstoryPanel : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            InitializeTextInfo();
            if (_hasBackground) InitializeStoryPanelWithBackground();

            StartBackStory();
        }
        private void OnDisable()
        {
            _typingTimer.OnTimerFinished -= OnCharacterTyping;
            _LineOnScreenTimer.OnTimerFinished -= OnLineScreenTimeOver;
        }

        // Update is called once per frame
        void Update()
        {
            if (_typingTimer.IsRunning()) _typingTimer.Tick();
            if (_LineOnScreenTimer.IsRunning()) _LineOnScreenTimer.Tick();
        }

        #region Utils

        private void InitializeTextInfo()
        {
            _typingSpeed = _typingSpeed / 100;

            _storyText.text = $"";
            _currentCharacterIndex = 0;
            _writer = _backStoryInfo.m_line.m_sentence;
            _timeOnScreenDelay = _backStoryInfo.m_line.m_screenTime;

            _typingTimer = new Timer(_typingSpeed);
            _typingTimer.OnTimerFinished += OnCharacterTyping;

            _LineOnScreenTimer = new Timer(_timeOnScreenDelay);
            _LineOnScreenTimer.OnTimerFinished += OnLineScreenTimeOver;
        }

        private void InitializeStoryPanelWithBackground()
        {
            _backStoryImage.sprite = _backStoryInfo.m_background;
        }

        private void StartBackStory()
        {
            Debug.Log("let's start this story");
            Sequence backstory = DOTween.Sequence();
            if (_hasBackground) backstory.Append(_imgBackgroundCanvas.DOFade(1, _backgroundImageFadeIn));
            backstory.Append(_textBackgroundCanvas.DOFade(1, _textBackgroundFadeIn));
            backstory.OnComplete(() => OnCharacterTyping());
        }

        private void StartTyping()
        {
            _isTyping = true;
        }

        private void OnCharacterTyping()
        {
            if (_currentCharacterIndex < _writer.Length)
            {
                _storyText.text += $"{_writer[_currentCharacterIndex]}";
                _currentCharacterIndex++;
                ResetTimer(_typingTimer);
            }
            else
            {
                _isTyping = false;
                ResetTimer(_LineOnScreenTimer);
            }
        }

        private void OnLineScreenTimeOver()
        {
            Sequence endOfStory = DOTween.Sequence();
            endOfStory.AppendCallback(() => DisableCanvases());
            endOfStory.AppendInterval(_intervalBetweenPanels).OnComplete(() => _onPanelStoryComplete.Raise());
        }

        private void DisableCanvases()
        {
            _textBackgroundCanvas.DOFade(0, _textFadeOut);
            _TextCanvas.DOFade(0, _textFadeOut);
        }

        private void ResetTimer(Timer timer)
        {
            timer.Reset();
            timer.Begin();
        }

        #endregion

        #region Privates & Protected

        private Timer _typingTimer;
        private Timer _LineOnScreenTimer;
        private int _currentCharacterIndex = 0;
        private bool _isTyping = false;
        private string _writer;

        [SerializeField] private bool _hasBackground;
        [SerializeField] private BackStoryInfo _backStoryInfo;
        [SerializeField] private TMP_Text _storyText;
        [SerializeField] private Image _backStoryImage;

        [Header("Text Specifics")]
        [SerializeField] private float _typingSpeed;
        private float _timeOnScreenDelay;

        [Header("Canvas Group for smoother intro")]
        [SerializeField] private CanvasGroup _imgBackgroundCanvas;
        [SerializeField] private CanvasGroup _textBackgroundCanvas;
        [SerializeField] private CanvasGroup _TextCanvas;

        [Header("Timing")]
        [SerializeField] private float _backgroundImageFadeIn;
        [SerializeField] private float _textBackgroundFadeIn;
        [SerializeField] private float _textFadeOut;
        [SerializeField] private float _intervalBetweenPanels;

        [Header("Event for next panel")]
        [SerializeField] private VoidEvent _onPanelStoryComplete;

        #endregion
    }
}
