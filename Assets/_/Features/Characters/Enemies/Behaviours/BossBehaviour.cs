using Data.Runtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies.Runtime
{
    public class BossBehaviour : MonoBehaviour
    {
        #region Publics

        [Header("Components Settings")]
        public GameObject m_gridBehaviour;
        public IGrid m_gridInterface;
        public GameObject m_player;
        public Slider m_healthBar;
        public float m_lifePoints;

        [Header("Attack Settings")]
        public Image m_waveAttack;
        public Image m_zoneAttack;
        public Image m_CollapseAttack;
        public float m_timerAttack;

        [Header("Timer Settings")]
        public float m_timeIdle;
        public float m_timeCollapseToIdle;
        public float m_timeZoneToIdle;

        [Header("Collapse Shake Settings")]
        public float m_shakingRandom;
        public int m_shakingVibration;
        public float m_shakingDuration;
        public Vector3 m_shakingStrength;

        [Header("Collapse Fall Settings")]
        public int m_collapseNumber;
        public float m_DelayRespawn;
        public float m_fallDuration;
        public float m_fallPositionY;

        [Header("Line Settings")]
        public GameObject m_wave;
        public float m_waveDistance;
        public float m_timeWaveLine;
        public float m_firstWaveSpeed;
        public float m_secondWaveSpeed;
        public float m_thirdWaveSpeed;

        [Header("Zone Danger Settings")]
        public GameObject m_dangerZone;
        public float m_growthSpeed;
        public int m_ZoneNumber;
        public List<GameObject> m_dangerZones;

        #endregion

        #region Unity

        private void Awake()
        {
            m_gridBehaviour.TryGetComponent(out m_gridInterface);

            DangerZoneList();
        }

        private void Start()
        {
            _stateMachine = new(this);
            _stateMachine.ChangeState(_stateMachine.m_bossState);

            m_healthBar.maxValue = m_lifePoints;
            m_healthBar.value = m_lifePoints;
        }

        private void Update()
        {
            _stateMachine.HandleInput();
            _stateMachine.Tick();
        }

        private void FixedUpdate() => _stateMachine.PhysicsTick();

        private void LateUpdate() => _stateMachine.FinalTick();

        #endregion

        #region Methods

        public void TakeDamage(int damage)
        {
            m_lifePoints -= damage;
            m_healthBar.value = m_lifePoints;

            if (m_healthBar.value <= 0) DeathBoss();
        }

        #endregion

        #region Utils

        private void DangerZoneList()
        {
            for (int i = 0; i < m_ZoneNumber; i++)
            {
                GameObject zone = Instantiate(m_dangerZone, transform);
                m_dangerZones.Add(zone);
            }
        }

        private void DeathBoss()
        {
            _stateMachine.ChangeState(_stateMachine.m_bossState);
            m_healthBar.gameObject.SetActive(false);
            m_CollapseAttack.gameObject.SetActive(false);
            m_waveAttack.gameObject.SetActive(false);
            m_zoneAttack.gameObject.SetActive(false);
            this.enabled = false;
            return;
        }

        #endregion

        #region Privates

        private StateMachine _stateMachine;

        #endregion
    }
}
