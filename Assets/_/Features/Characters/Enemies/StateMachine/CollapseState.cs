using Data.Runtime;
using DG.Tweening;
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

            _plateform = _bossBehaviour.m_gridInterface.GetRandomPlateform();
            _plateformOriginPosition = _plateform.transform.position;
            _plateform.TryGetComponent(out _plateformMeshRenderer);

            if (!_plateformMeshRenderer) throw new System.Exception($"{_plateform.name} is missing a mesh renderer!");

            FallSequence();
        }

        public void Exit()
        {
            m_timer?.Reset();
            m_timer.OnTimerFinished -= ChangeState;
        }

        public void Tick()
        {
            m_timer?.Tick();
        }

        public void PhysicsTick()
        {

        }

        public void FinalTick()
        {

        }

        public void HandleInput()
        {

        }

        #endregion

        #region Utils

        private void FallSequence()
        {
            _fallSequence = DOTween.Sequence();

            _fallSequence.Append(_plateform.transform.DOShakePosition(
                _bossBehaviour.m_shakingDuration,
                _bossBehaviour.m_shakingStrength, 
                _bossBehaviour.m_shakingVibration, 
                _bossBehaviour.m_shakingRandom
            ));
            _fallSequence.Append(_plateform.transform.DOMoveY(_bossBehaviour.m_fallPositionY, _bossBehaviour.m_fallDuration));
            _fallSequence.Append(HideOrShowPlatform(_bossBehaviour.m_hideMaterialAplha, _bossBehaviour.m_durationFadeOut));
            _fallSequence.AppendInterval(_bossBehaviour.m_DelayRespawn);
            _fallSequence.AppendCallback(RespawnAfterAWhile);
        }

        private Tween HideOrShowPlatform(float fadeColor, float fadeTime) => _plateformMeshRenderer.material.DOFade(fadeColor, fadeTime);

        private void RespawnAfterAWhile()
        {
            _plateform.transform.position = _plateformOriginPosition;
            _plateformMeshRenderer.material.DOFade(_bossBehaviour.m_showMaterialAlpha, _bossBehaviour.m_durationFadeIn);

            m_timer?.Begin();
        }

        private void ChangeState() => m_stateMachine.ChangeState(m_stateMachine.m_bossState);

        #endregion

        #region Privates

        private BossBehaviour _bossBehaviour;
        private Sequence _fallSequence;
        private GameObject _plateform;

        private Vector3 _plateformOriginPosition;
        private MeshRenderer _plateformMeshRenderer;

        private Timer m_timer;

        #endregion
    }
}
