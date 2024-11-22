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

            _timer = new(_bossBehaviour.m_timeCollapseToIdle);
            _timerAttack = new(_bossBehaviour.m_timerAttack);
        }

        public void Enter()
        {
            _timerAttack?.Reset();
            _timerAttack?.Begin();
            _bossBehaviour.m_CollapseAttack.enabled = true;
            _timerAttack.OnTimerFinished += FallSequence;

            _timer.OnTimerFinished += ChangeState;
            _timer?.Reset();

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
        }

        public void Exit()
        {
            _plateforms.Clear();
            _originalPositions.Clear();  
            _meshRenderers.Clear();

            _timerAttack.OnTimerFinished -= FallSequence;
            _timer.OnTimerFinished -= ChangeState; 
        }

        public void Tick()
        {
            _timerAttack?.Tick();

            if (_timerAttack.IsRunning()) return;
            else if (_bossBehaviour.m_CollapseAttack.enabled) _bossBehaviour.m_CollapseAttack.enabled = false;

            _timer?.Tick();
        }

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
                _fallSequence.AppendInterval(_bossBehaviour.m_DelayRespawn);
                _fallSequence.AppendCallback(() => RespawnPlatform(platform, meshRenderer, originalPosition));
            }
        }

        private void RespawnPlatform(GameObject platform, MeshRenderer meshRenderer, Vector3 originalPosition)
        {
            platform.transform.position = originalPosition;

            if (!_timer.IsRunning()) _timer?.Begin();
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

        private Timer _timer, _timerAttack;

        #endregion
    }
}
