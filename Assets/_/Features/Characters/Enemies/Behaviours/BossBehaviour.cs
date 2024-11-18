using Data.Runtime;
using UnityEngine;

namespace Enemies.Runtime
{
    public class BossBehaviour : MonoBehaviour
    {
        #region Publics

        [Header("Timer Settings")]
        public float m_timeIdle;
        public float m_timeCollapseToIdle;

        [Header("Collapse Shake Settings")]
        public float m_shakingRandom;
        public int m_shakingVibration;
        public float m_shakingDuration;
        public Vector3 m_shakingStrength;

        [Header("Collapse Fall Settings")]
        public float m_DelayRespawn;
        public float m_fallDuration;
        public float m_fallPositionY;

        [Header("Collapse Material Settings")]
        public float m_hideMaterialAplha;
        public float m_showMaterialAlpha;
        public float m_durationFadeOut;
        public float m_durationFadeIn;

        [Header("Grid Settings")]
        public GameObject m_gridBehaviour;
        public IGrid m_gridInterface;

        #endregion

        #region Unity

        private void Awake() => m_gridBehaviour.TryGetComponent(out m_gridInterface);

        private void Start()
        {
            _stateMachine = new(this);
            _stateMachine.ChangeState(_stateMachine.m_bossState);
        }

        private void Update()
        {
            _stateMachine.HandleInput();
            _stateMachine.Tick();
        }

        private void FixedUpdate()
        {
            _stateMachine.PhysicsTick();
        }

        private void LateUpdate()
        {
            _stateMachine.FinalTick();
        }

        #endregion

        #region Privates

        private StateMachine _stateMachine;

        #endregion
    }
}
