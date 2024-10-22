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

        }

        void Start()
    	{
            _spellImage.color = _spellList[0];

            for (int i = 0; i < _maxLives; i++)
            {
                Debug.Log("hello");
                GameObject life = Instantiate(_livesPrefab, _livesTransform);
                _livesList.Add(life);
            }
        }

    	void Update()
    	{
	
    	}

        #endregion


        #region Main Methods

        [ContextMenu("Test Fade In and Out")]
        private void FadeOnTeleportation()
        {
            Sequence fadeSequence = DOTween.Sequence();
            fadeSequence.Append(_teleportBlackScreen.DOFade(1, _teleportFadeInDelay));
            fadeSequence.AppendInterval(_teleportIntervalDelay);
            fadeSequence.Append(_teleportBlackScreen.DOFade(0, _teleportFadeOutDelay));
        }

        [ContextMenu("update spell")]
        private void TestColor()
        {
            UpdateSpellImage(_spell);
        }

        public void UpdateSpellImage(int color)
        {
            _spellImage.color = _spellList[color];
        }

        #endregion


        #region Utils

        #endregion


        #region Privates & Protected

        [Header("Teleport Black Screen Settings")]
        [SerializeField] private CanvasGroup _teleportBlackScreen;
        [SerializeField] private float _teleportFadeInDelay = 1f;
        [SerializeField] private float _teleportIntervalDelay = .2f;
        [SerializeField] private float _teleportFadeOutDelay = 1.5f;

        [Header("Lives")]
        [SerializeField] private GameObject _livesPrefab;
        [SerializeField] private Transform _livesTransform;
        [SerializeField] private int _maxLives;
        [SerializeField] private List<GameObject> _livesList;

        [Header("Spells")]
        [SerializeField] private List<Color> _spellList = new List<Color>();
        [SerializeField] private Image _spellImage;
        [SerializeField] private int _spell;

        #endregion
    }
}