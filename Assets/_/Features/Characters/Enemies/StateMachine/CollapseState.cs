using Data.Runtime;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Runtime
{
    public class CollapseState : IState
    {
        #region Publics

        public StateMachine m_stateMachine { get; }

        #endregion

        #region Methods

        public CollapseState(StateMachine stateMachine)
        {
            m_stateMachine = stateMachine;
            _bossBehaviour = m_stateMachine.m_bossBehaviour;

            m_timer = new(_bossBehaviour.m_timeCollapseToIdle);
        }

        public void Enter()
        {
            m_timer.OnTimerFinished += ChangeState;
            m_timer?.Reset();

            HashSet<GameObject> selectedPlatforms = new();

            while (selectedPlatforms.Count < _bossBehaviour.m_collapseNumber)
            {
                GameObject platform = _bossBehaviour.m_gridInterface.GetRandomPlatform();
                if (platform != null) selectedPlatforms.Add(platform);
            }

            foreach (var platform in selectedPlatforms)
            {
                _plateforms.Add(platform);
                _originalPositions.Add(platform.transform.position);

                if (platform.TryGetComponent(out MeshRenderer meshRenderer)) _meshRenderers.Add(meshRenderer);
                else throw new System.Exception($"{platform.name} is missing a mesh renderer!");
            }

            FallSequence();
        }

        public void Exit()
        {
            _plateforms.Clear();
            _originalPositions.Clear();  
            _meshRenderers.Clear();

            m_timer.OnTimerFinished -= ChangeState; 
        }

        public void Tick() => m_timer?.Tick(); 

        public void PhysicsTick() { }

        public void FinalTick() { }

        public void HandleInput() { }

        #endregion

        #region Utils

        private void FallSequence()
        {
            for (int i = 0; i < _bossBehaviour.m_collapseNumber; i++)
            {
                _fallSequence = DOTween.Sequence();

                GameObject platform = _plateforms[i];
                MeshRenderer meshRenderer = _meshRenderers[i];
                Vector3 originalPosition = _originalPositions[i];

                _fallSequence.Append(platform.transform.DOShakePosition(
                    _bossBehaviour.m_shakingDuration,
                    _bossBehaviour.m_shakingStrength,
                    _bossBehaviour.m_shakingVibration,
                    _bossBehaviour.m_shakingRandom
                ));

                _fallSequence.Append(platform.transform.DOMoveY(_bossBehaviour.m_fallPositionY, _bossBehaviour.m_fallDuration));
                _fallSequence.Append(HideOrShowPlatform(meshRenderer, _bossBehaviour.m_hideMaterialAlpha, _bossBehaviour.m_durationFadeOut));
                _fallSequence.AppendInterval(_bossBehaviour.m_DelayRespawn);
                _fallSequence.AppendCallback(() => RespawnPlatform(platform, meshRenderer, originalPosition));
            }
        }

        private Tween HideOrShowPlatform(MeshRenderer meshRenderer, float fadeColor, float fadeTime) =>
            meshRenderer.material.DOFade(fadeColor, fadeTime);

        private void RespawnPlatform(GameObject platform, MeshRenderer meshRenderer, Vector3 originalPosition)
        {
            platform.transform.position = originalPosition;
            meshRenderer.material.DOFade(_bossBehaviour.m_showMaterialAlpha, _bossBehaviour.m_durationFadeIn);

            if (!m_timer.IsRunning()) m_timer?.Begin();
        }

        private void ChangeState() => m_stateMachine.ChangeState(m_stateMachine.m_bossState);

        #endregion

        #region Privates

        private BossBehaviour _bossBehaviour;
        private Sequence _fallSequence;
        private GameObject _plateform;

        private Vector3 _plateformOriginPosition;
        private MeshRenderer _plateformMeshRenderer;

        private List<GameObject> _plateforms = new();
        private List<Vector3> _originalPositions = new();
        private List<MeshRenderer> _meshRenderers = new();

        private Timer m_timer;

        #endregion
    }
}
