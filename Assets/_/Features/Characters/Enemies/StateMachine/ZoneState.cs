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
            
            _timer = new(_bossBehaviour.m_timeZoneToIdle);
            _timerAttack = new(_bossBehaviour.m_timerAttack);
        }

        public void Enter()
        {
            _timerAttack?.Reset();
            _timerAttack?.Begin();
            _bossBehaviour.m_zoneAttack.enabled = true;

            _timer.OnTimerFinished += ChangeState;
            _timer?.Reset();

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

                    Vector3 targetScale = new Vector3(platformMeshRenderer.bounds.size.x, platformMeshRenderer.bounds.size.x, platformMeshRenderer.bounds.size.z);
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

            _timer.OnTimerFinished -= ChangeState;
        }

        public void Tick()
        {
            _timerAttack?.Tick();

            if (_timerAttack.IsRunning()) return;
            else if (_bossBehaviour.m_zoneAttack.enabled) _bossBehaviour.m_zoneAttack.enabled = false;

            _timer?.Tick();

            for (int i = 0; i < _activeDangerZones.Count; i++)
            {
                GameObject dangerZone = _activeDangerZones[i];
                Vector3 currentScale = dangerZone.transform.localScale;
                Vector3 targetScale = _targetScales[i];

                if (currentScale.x < targetScale.x || currentScale.y < targetScale.y || currentScale.z < targetScale.z)
                {
                    currentScale.x = Mathf.Min(targetScale.x, currentScale.x + Time.deltaTime * _growthSpeed);
                    currentScale.y = Mathf.Min(targetScale.y, currentScale.y + Time.deltaTime * _growthSpeed);
                    currentScale.z = Mathf.Min(targetScale.z, currentScale.z + Time.deltaTime * _growthSpeed);
                    dangerZone.transform.localScale = currentScale;
                }
            }

            if (_activeDangerZones.TrueForAll(dz => 
                dz.transform.localScale.x >= _targetScales[_activeDangerZones.IndexOf(dz)].x &&
                dz.transform.localScale.y >= _targetScales[_activeDangerZones.IndexOf(dz)].y &&
                dz.transform.localScale.z >= _targetScales[_activeDangerZones.IndexOf(dz)].z)) 
                    if (!_timer.IsRunning()) _timer?.Begin();
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

        private Timer _timer, _timerAttack;

        #endregion
    }
}
