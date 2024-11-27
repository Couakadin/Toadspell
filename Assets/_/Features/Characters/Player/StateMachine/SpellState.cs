using Data.Runtime;
using UnityEngine;

namespace Player.Runtime
{
    public class SpellState : IState
    {
        #region Publics

        public StateMachine m_stateMachine { get; }

        #endregion
        

        #region Methods

        public SpellState(StateMachine stateMachine) 
        {
            m_stateMachine = stateMachine;

            _timerState = new(m_stateMachine.m_powerBehaviour.m_spellFireRate);
            _timerSpell = new(m_stateMachine.m_powerBehaviour.m_castingASpellDelay);
            _playerTransform = m_stateMachine.m_powerBehaviour.transform;
        }

        public void Enter()
        {
            _timerSpell?.Reset();
            _timerState?.Reset();

            _timerSpell?.Begin();
            _timerState?.Begin();

            // Timer
            _timerSpell.OnTimerFinished += CastSpell;
            _timerState.OnTimerFinished += ChangeState;

            // Target
            _target = m_stateMachine.m_powerBehaviour.m_tongueBlackboard.GetValue<GameObject>("CurrentLockedTarget");
            _currentPool = m_stateMachine.m_powerBehaviour.m_currentPool;

            if (_target == null || _currentPool == null) { ChangeState(); return; }
            if (!_target && !_projectile) { ChangeState(); return; }

            m_stateMachine.m_powerBehaviour.m_playerAnimator.SetLayerWeight(2, .7f); // Attack Layer
            m_stateMachine.m_powerBehaviour.m_playerAnimator.SetBool("IsAttack", true);

            m_stateMachine.m_powerBehaviour.m_onSpellTimerTrigger.Raise();
        }

        public void Exit()
        {
            _timerSpell?.Reset();
            _timerState?.Reset();

            _timerSpell.OnTimerFinished -= CastSpell;
            _timerState.OnTimerFinished -= ChangeState;

            m_stateMachine.m_powerBehaviour.m_playerAnimator.SetLayerWeight(2, 0f);
            m_stateMachine.m_powerBehaviour.m_playerAnimator.SetBool("IsAttack", false);
        }

        public void Tick()
        {
            _timerSpell?.Tick();
            _timerState?.Tick();

            if (_timerSpell.IsRunning()) return;
            if (_targetCollider != null)
            {
                Vector3 targetCenter = _targetCollider.bounds.center;
                _distanceToTarget = targetCenter - _projectile.transform.position;
            }
            else _distanceToTarget = _target.transform.position - _projectile.transform.position;

            if (_distanceToTarget.sqrMagnitude > .5f * .5f) _projectileRigidbody.velocity = m_stateMachine.m_powerBehaviour.m_speedOfProjectile * _distanceToTarget;
            else ChangeState();
        }

        public void PhysicsTick() { }

        public void FinalTick() { }

        public void HandleInput() { }

        #endregion


        #region Utils

        private void CastSpell()
        {
            // Pool
           
            _projectile = _currentPool?.GetFirstAvailableObject();
            _projectile.transform.position = m_stateMachine.m_powerBehaviour._playerBlackboard.GetValue<Vector3>("SpellPosition");
            _projectile.TryGetComponent(out _projectileRigidbody);
            _projectileRigidbody.velocity = Vector3.zero;
            m_stateMachine.m_powerBehaviour.m_onSpellCast.Raise();

            _playerTransform.LookAt(new Vector3(_target.transform.position.x, _playerTransform.position.y, _target.gameObject.transform.position.z));
            m_stateMachine.m_powerBehaviour.CastASpell(); // sound of casting a spell
            _target.TryGetComponent(out _targetCollider);
            if (!_targetCollider) throw new System.Exception("No Target Collider!");
        }

        private void ChangeState() => m_stateMachine.ChangeState(m_stateMachine.m_lockState);

        #endregion


        #region Privates

        private PoolSystem _currentPool;
        private GameObject _projectile;
        private Rigidbody _projectileRigidbody;

        private GameObject _target;
        private PlayerSoundBehaviour _soundBehaviour;
        private Collider _targetCollider;
        private Transform _playerTransform;

        private Vector3 _distanceToTarget;

        private Timer _timerSpell, _timerState;

        #endregion
    }
}
