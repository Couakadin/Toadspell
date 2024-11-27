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
            _timerStill = new(1f);
            _timerAttack = new(_bossBehaviour.m_timerAttack);
        }

        public void Enter()
        {
            _timerAttack?.Reset();
            _timerAttack?.Begin();
            _bossBehaviour.m_waveAttack.enabled = true;

            if (_index <= 0) _index = 3;

            if (PlayerDetected(out Vector3 playerPosition))
                _direction = GetPlayerDirection(playerPosition);
            else
                _direction = GetRandomDirection();

            _wave.transform.position = new Vector3(
                _gridInterface.m_centralPlatform.transform.position.x, 
                _gridInterface.m_centralPlatform.transform.position.y + m_stateMachine.m_bossBehaviour.m_waveOffset, 
                _gridInterface.m_centralPlatform.transform.position.z
            );

            if (_direction == Vector3.forward) _wave.transform.rotation = Quaternion.Euler(0, 270f, 0);
            else if (_direction == Vector3.back) _wave.transform.rotation = Quaternion.Euler(0, 90f, 0);
            else if (_direction == Vector3.right) _wave.transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (_direction == Vector3.left) _wave.transform.rotation = Quaternion.Euler(0, 180f, 0);

            _timerStill.OnTimerFinished += StartReturningToCenter;
            _timerStill?.Reset();
            _timerWave?.Begin();
        }

        public void Exit()
        {
            _timerWave?.UpdateTimer(_bossBehaviour.m_timeWaveLine - 1);

            _wave.SetActive(false);
            _timerStill.OnTimerFinished -= StartReturningToCenter;
        }

        public void Tick() 
        {
            _timerAttack?.Tick();

            if (_timerAttack.IsRunning()) return;
            else if (_bossBehaviour.m_waveAttack.enabled) _bossBehaviour.m_waveAttack.enabled = false;

            _timerWave?.Tick();
            _timerStill?.Tick();

            if (!_timerWave.IsRunning() && _index > 0) MoveWave();
        }

        public void PhysicsTick() { }

        public void FinalTick() { }

        public void HandleInput() { }

        #endregion

        #region Utils

        private Vector3 GetPlayerDirection(Vector3 playerPosition)
        {
            Vector3 rawDirection = playerPosition - _gridInterface.m_centralPlatform.transform.position;

            if (Mathf.Abs(rawDirection.x) > Mathf.Abs(rawDirection.z)) return rawDirection.x > 0 ? Vector3.right : Vector3.left;
            else return rawDirection.z > 0 ? Vector3.forward : Vector3.back;
        }

        internal bool PlayerDetected(out Vector3 playerPosition)
        {
            if (_bossBehaviour.m_player != null)
            {
                playerPosition = _bossBehaviour.m_player.transform.position;
                return true;
            }

            playerPosition = GetRandomDirection();
            return false;
        }

        private Vector3 GetRandomDirection()
        {
            Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
            int randomIndex = Random.Range(0, directions.Length);
            return directions[randomIndex];
        }

        private void MoveWave()
        {
            if (!_wave.activeSelf)_wave.SetActive(true);

            if (_index == 3) _waveSpeed = _bossBehaviour.m_firstWaveSpeed;
            else if (_index == 2) _waveSpeed = _bossBehaviour.m_secondWaveSpeed;
            else if (_index == 1) _waveSpeed = _bossBehaviour.m_thirdWaveSpeed;

            Vector3 targetDirection = (_gridInterface.m_centralPlatform.transform.position + _direction * 100f) - _wave.transform.position;
            targetDirection.Normalize();

            _waveBody.velocity = targetDirection * _waveSpeed;

            Vector3 distanceToCenter = _wave.transform.position - _gridInterface.m_centralPlatform.transform.position;
            if (distanceToCenter.sqrMagnitude > _bossBehaviour.m_waveDistance)
            {
                _waveBody.velocity = Vector3.zero;
                _timerStill?.Begin();
            }
        }

        private void StartReturningToCenter()
        {
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
        private Timer _timerWave, _timerStill, _timerAttack;
        private Rigidbody _waveBody;
        private float _waveSpeed;

        #endregion
    }
}
