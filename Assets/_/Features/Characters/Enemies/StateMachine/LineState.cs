using Data.Runtime;
using UnityEngine;

namespace Enemies.Runtime
{
    public class LineState : IState
    {
        #region Publics

        public StateMachine m_stateMachine { get; }

        #endregion

        #region Methods

        public LineState(StateMachine stateMachine)
        {
            m_stateMachine = stateMachine;
            _bossBehaviour = m_stateMachine.m_bossBehaviour;
            _gridInterface = _bossBehaviour.m_gridInterface;
            _wave = _bossBehaviour.m_wave;
            _wave.TryGetComponent(out _waveBody);

            _timerWave = new(_bossBehaviour.m_timeWaveLine);
        }

        public void Enter()
        {
            if (_index <= 0) _index = 3;

            _direction = GetRandomDirection();
            
            _waveBody.velocity = Vector3.zero;
            _wave.transform.position = _gridInterface.m_centralPlatform.transform.position;
            if (_direction == Vector3.forward) _wave.transform.rotation = Quaternion.Euler(0, 270f, 0);
            else if (_direction == Vector3.back) _wave.transform.rotation = Quaternion.Euler(0, 90f, 0);
            else if (_direction == Vector3.right) _wave.transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (_direction == Vector3.left) _wave.transform.rotation = Quaternion.Euler(0, 180f, 0);
            _wave.SetActive(true);

            _timerWave?.Reset();
            _timerWave?.Begin();
        }

        public void Exit() => _wave.SetActive(false);

        public void Tick() 
        {
            _timerWave?.Tick();

            MoveWave();
        }

        public void PhysicsTick() { }

        public void FinalTick() { }

        public void HandleInput() { }

        #endregion

        #region Utils

        private Vector3 GetRandomDirection()
        {
            Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
            int randomIndex = Random.Range(0, directions.Length);
            return directions[randomIndex];
        }

        private void MoveWave()
        {
            if (_index == 3) _waveSpeed = _bossBehaviour.m_firstWaveSpeed;
            else if (_index == 2) _waveSpeed = _bossBehaviour.m_secondWaveSpeed;
            else if (_index == 1) _waveSpeed = _bossBehaviour.m_thirdWaveSpeed;
            float t = _timerWave.GetRemainingTime() * _waveSpeed;

            Vector3 targetDirection = (_gridInterface.m_centralPlatform.transform.position + _direction * 100f) - _wave.transform.position;
            targetDirection.Normalize();

            _waveBody.velocity = targetDirection * t;

            Vector3 distanceToCenter = _wave.transform.position - _gridInterface.m_centralPlatform.transform.position;
            if (distanceToCenter.sqrMagnitude > _waveBody.velocity.sqrMagnitude) StartReturningToCenter();
        }

        private void StartReturningToCenter()
        {
            _wave.SetActive(false);
            _index--;

            if (_index <= 0)
            {
                ChangeState();
                return;
            }

            m_stateMachine.ChangeState(m_stateMachine.m_lineState);
        }

        private void ChangeState() => m_stateMachine.ChangeState(m_stateMachine.m_bossState);

        #endregion

        #region Privates

        private BossBehaviour _bossBehaviour;
        private IGrid _gridInterface;

        private Vector3 _direction;
        private int _index = 3;

        private GameObject _wave;
        private Timer _timerWave;
        private Rigidbody _waveBody;
        private float _waveSpeed;

        #endregion
    }
}
