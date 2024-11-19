using Data.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Runtime
{
    public class ZoneState : IState
    {
        #region Publics

        public StateMachine m_stateMachine { get; }

        #endregion

        #region Methods

        public ZoneState(StateMachine stateMachine)
        {
            m_stateMachine = stateMachine;
            _bossBehaviour = m_stateMachine.m_bossBehaviour;

            _dangerZones = _bossBehaviour.m_dangerZones;
            _growthSpeed = _bossBehaviour.m_growthSpeed;
            
            m_timer = new(_bossBehaviour.m_timeZoneToIdle);
        }

        public void Enter()
        {
            m_timer.OnTimerFinished += ChangeState;
            m_timer?.Reset();

            HashSet<GameObject> selectedPlatforms = new HashSet<GameObject>();
            while (selectedPlatforms.Count < _dangerZones.Count)
            {
                GameObject platform = _bossBehaviour.m_gridInterface.GetRandomPlatform();
                if (platform != null) selectedPlatforms.Add(platform);
            }

            int index = 0;
            foreach (GameObject platform in selectedPlatforms)
            {
                if (platform.TryGetComponent(out MeshRenderer platformMeshRenderer))
                {
                    GameObject dangerZone = _dangerZones[index];
                    dangerZone.transform.localScale = Vector3.zero;

                    Vector3 targetScale = new Vector3(platformMeshRenderer.bounds.size.x, dangerZone.transform.localScale.y, platformMeshRenderer.bounds.size.z);
                    dangerZone.transform.position = new Vector3(
                        platform.transform.position.x,
                        platform.transform.position.y + (platformMeshRenderer.bounds.size.y + .5f),
                        platform.transform.position.z
                    );

                    _activeDangerZones.Add(dangerZone);
                    _targetScales.Add(targetScale);
                    dangerZone.SetActive(true);

                    index++;
                }
            }
        }

        public void Exit()
        {
            foreach (var dangerZone in _activeDangerZones)
            {
                dangerZone.transform.localScale = Vector3.zero;
                dangerZone.SetActive(false);
            }

            _activeDangerZones.Clear();
            _targetScales.Clear();

            m_timer.OnTimerFinished -= ChangeState;
        }

        public void Tick()
        {
            m_timer?.Tick();

            for (int i = 0; i < _activeDangerZones.Count; i++)
            {
                GameObject dangerZone = _activeDangerZones[i];
                Vector3 currentScale = dangerZone.transform.localScale;
                Vector3 targetScale = _targetScales[i];

                if (currentScale.x < targetScale.x || currentScale.z < targetScale.z)
                {
                    currentScale.x = Mathf.Min(targetScale.x, currentScale.x + Time.deltaTime * _growthSpeed);
                    currentScale.z = Mathf.Min(targetScale.z, currentScale.z + Time.deltaTime * _growthSpeed);
                    dangerZone.transform.localScale = currentScale;
                }
            }

            if (_activeDangerZones.TrueForAll(dz => 
            dz.transform.localScale.x >= _targetScales[_activeDangerZones.IndexOf(dz)].x && 
            dz.transform.localScale.z >= _targetScales[_activeDangerZones.IndexOf(dz)].z)) if (!m_timer.IsRunning()) m_timer?.Begin();
        }

        public void PhysicsTick() { }

        public void FinalTick() { }

        public void HandleInput() { }

        #endregion

        #region Utils

        private void ChangeState() => m_stateMachine.ChangeState(m_stateMachine.m_bossState);

        #endregion

        #region Privates

        private BossBehaviour _bossBehaviour;

        private GameObject _dangerZonePrefab;
        private float _growthSpeed;

        private List<GameObject> _plateform;
        private MeshRenderer _plateformMeshRenderer;

        private List<GameObject> _activeDangerZones = new();
        private List<GameObject> _dangerZones = new();
        private List<Vector3> _targetScales = new();

        private Timer m_timer;

        #endregion
    }
}
