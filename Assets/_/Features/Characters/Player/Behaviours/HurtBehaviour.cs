using Data.Runtime;
using UnityEngine;

namespace Player.Runtime
{
    public class HurtBehaviour : MonoBehaviour, IHurt
    {
        #region Unity

        private void Awake()
        {
            TryGetComponent(out _moveBehaviour);
            TryGetComponent(out _characterController);
        }

        private void Update()
        {
            _impactTimer.Tick();

            if (!_impactTimer.IsRunning()) MoveStart();

            // consumes the impact energy each cycle
            if (m_impact == Vector3.zero) return;
            m_impact = Vector3.Lerp(m_impact, Vector3.zero, 5 * Time.deltaTime);

            // apply the impact force
            if (m_impact.sqrMagnitude > (.2f * .2f))
                _characterController.Move(m_impact * Time.deltaTime);
        }

        #endregion

        #region Methods

        // call this function to add an impact force
        public void AddImpact(Vector3 dir, float force)
        {
            MoveStop();
            _impactTimer.Reset();
            _impactTimer.Begin();

            dir.Normalize();
            if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
            m_impact += dir * force / m_mass;
        }

        #endregion

        #region Utils

        private void MoveStart() => _moveBehaviour.enabled = true;
        private void MoveStop() => _moveBehaviour.enabled = false;

        #endregion

        #region Privates

        private MoveBehaviour _moveBehaviour;
        private CharacterController _characterController;
        private Timer _impactTimer = new (.2f);

        private float m_mass = 3f;
        private Vector3 m_impact = Vector3.zero;

        #endregion
    }
}
