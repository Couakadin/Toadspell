using Data.Runtime;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class UIManager : MonoBehaviour
    {
        #region Publics

        #endregion


        #region Unity API

        private void Awake()
        {
            _maxLives = (int) _playerBlackboard.GetValue<float>("Lives");
        }

        void Start()
    	{
            _spellImage.color = _spellList[0];

            for (int i = 0; i < _maxLives; i++)
            {
                GameObject life = Instantiate(_livesPrefab, _livesTransform);
                _livesList.Add(life);
            }
        }

    	void Update()
    	{

    	}

        #endregion


        #region Main Methods

        public void FadeOnTeleportation()
        {
            Sequence fadeSequence = DOTween.Sequence();
            fadeSequence.Append(_teleportBlackScreen.DOFade(1, _teleportFadeInDelay));
            fadeSequence.AppendInterval(_teleportIntervalDelay);
            fadeSequence.Append(_teleportBlackScreen.DOFade(0, _teleportFadeOutDelay));
        }

        public void UpdateSpellImage(int spell)
        {
            _spellImage.color = _spellList[spell];
        }

        public void UpdateLives()
        {
            _currentLives = (int)_playerBlackboard.GetValue<float>("Lives");
            if (_currentLives < 0) { _currentLives = 0; }
            for (int i = 0; i < _livesList.Count; i++)
            {
                _livesList[i].SetActive(false);
            }
            for(int i = 0; i < _currentLives; i++)
            {
                _livesList[i].SetActive(true);
            }
        }

        #endregion


        #region Utils

        #endregion


        #region Privates & Protected

        [Header("References")]
        [SerializeField] private Blackboard _playerBlackboard;

        [Header("Teleport Black Screen Settings")]
        [SerializeField] private CanvasGroup _teleportBlackScreen;
        [SerializeField] private float _teleportFadeInDelay = 1f;
        [SerializeField] private float _teleportIntervalDelay = .2f;
        [SerializeField] private float _teleportFadeOutDelay = 1.5f;

        [Header("Lives")]
        [SerializeField] private GameObject _livesPrefab;
        [SerializeField] private Transform _livesTransform;
        private int _maxLives;
        private int _currentLives;
        [SerializeField] private List<GameObject> _livesList;

        [Header("Spells")]
        [SerializeField] private List<Color> _spellList = new List<Color>();
        [SerializeField] private Image _spellImage;
        [SerializeField] private int _spell;

        #endregion
    }
}