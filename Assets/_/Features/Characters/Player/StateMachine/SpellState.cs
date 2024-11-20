using Data.Runtime;
using UnityEditorInternal;
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
            _timer = new Timer(m_stateMachine.m_powerBehaviour.m_durationOfProjectile);
        }

        public void Enter()
        {
            // Target
            _target = m_stateMachine.m_powerBehaviour.m_tongueBlackboard.GetValue<GameObject>("CurrentLockedTarget");
            _currentPool = m_stateMachine.m_powerBehaviour.m_currentPool;

            if (_target == null || _currentPool == null)
            {
                ChangeState();
                return;
            }

            m_stateMachine.m_powerBehaviour.m_playerAnimator.SetLayerWeight(2, 1f); // Attack Layer
            m_stateMachine.m_powerBehaviour.m_playerAnimator.SetBool("IsAttack", true);
            m_stateMachine.m_powerBehaviour.CastASpell();

            // Pool
            _projectile = _currentPool?.GetFirstAvailableObject();
            _projectile.transform.position = m_stateMachine.m_powerBehaviour._playerBlackboard.GetValue<Vector3>("SpellPosition");
            _projectile.TryGetComponent(out _projectileRigidbody);
            _projectile.SetActive(true);

            // Timer
            _timer.OnTimerFinished += ChangeState;
            _timer.Begin();
        }

        public void Exit()
        {
            _projectile?.SetActive(false);

            _timer?.Reset();
            _timer.OnTimerFinished -= ChangeState;
            m_stateMachine.m_powerBehaviour.m_playerAnimator.SetBool("IsAttack", false);
        }

        public void Tick()
        {
            _timer.Tick();

            if (_target && _projectile)
            {
                // Obtenir le collider de l'ennemi
                _target.TryGetComponent(out Collider targetCollider);
                if (targetCollider)
                {
                    // Calculer le centre de l'ennemi en utilisant le collider
                    Vector3 targetCenter = targetCollider.bounds.center;
                    _distanceToTarget = targetCenter - _projectile.transform.position;
                }
                else
                {
                    // Si le collider n'est pas trouv�, utiliser une hauteur par d�faut
                    _distanceToTarget = _target.transform.position - _projectile.transform.position;
                }
            }

            if (_distanceToTarget.sqrMagnitude > .5f * .5f) _projectileRigidbody.velocity = m_stateMachine.m_powerBehaviour.m_speedOfProjectile * _distanceToTarget;
            else ChangeState();

        }

        public void PhysicsTick() { }

        public void FinalTick() { }

        public void HandleInput() { }

        #endregion

        #region Utils

        private void ChangeState() => m_stateMachine.ChangeState(m_stateMachine.m_lockState);

        #endregion

        #region Privates

        private PoolSystem _currentPool;
        private GameObject _projectile;
        private Rigidbody _projectileRigidbody;
        private GameObject _target;
        private Timer _timer;
        private PlayerSoundBehaviour _soundBehaviour;

        private Vector3 _distanceToTarget;

        #endregion
    }
}
