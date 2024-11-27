using Data.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.Runtime
{
    public class DissolveBehaviour : MonoBehaviour, ICanDissolve
    {
        #region Unity API

        private void Awake()
        {
            //TryGetComponent(out _plantAnimator);
            _cutoutFullPropertyBlock = new MaterialPropertyBlock();
            _cutoutFullPropertyBlock.SetFloat("_Cutout", 0.0f);

            _cutoutDissolvedPropertyBlock = new MaterialPropertyBlock();

        }

        private void Start()
        {
            _renderer.SetPropertyBlock(_cutoutFullPropertyBlock);
            foreach (Renderer renderer in _bodyRenderers)
            {
                renderer.material = _renderer.material;
            }
        }

        void Update()
    	{
            if (_isDying) EffectsWhenDying();
        }

        #endregion


        #region Main Methods

        public void StartDissolve()
        {
            _isDying = true;
            _elapsedTime = 0;
            _timeToDissolve = _dissolveTime;
            gameObject.layer = 0;

            if (_leaves == null) return;
            for(int i = 0; i < _leaves.Count; i++)
            {
                _leaves[i].SetActive(false);
            }
        }

        #endregion


        #region Utils

        private void EffectsWhenDying()
        {

            _elapsedTime += Time.deltaTime;

            float elapsedPercentage = _elapsedTime / _timeToDissolve;
            elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);

            _cutoutDissolvedPropertyBlock.SetFloat("_Cutout", elapsedPercentage);
            _renderer.SetPropertyBlock(_cutoutDissolvedPropertyBlock);

            foreach (Renderer renderer in _bodyRenderers)
            {
                renderer.SetPropertyBlock(_cutoutDissolvedPropertyBlock);
            }

            if (elapsedPercentage >= 1f)
            {
                gameObject.SetActive(false);
            }
        }

        #endregion


        #region Privates & Protected

        MaterialPropertyBlock _cutoutFullPropertyBlock;
        MaterialPropertyBlock _cutoutDissolvedPropertyBlock;
        
        [Header("References")]
        [SerializeField] private float _dissolveTime = 10f;
        [SerializeField] private MeshRenderer _renderer;

        [Header("Dissolve Renderers")]
        [SerializeField] private List<Renderer> _bodyRenderers;

        [Header("Archer Leaves")]
        [SerializeField] private List<GameObject> _leaves = new List<GameObject>();

        private float _elapsedTime;
        private float _timeToDissolve;
        private bool _isDying = false;

        #endregion
    }
}