using Data.Runtime;
using Enemies.Runtime;
using UnityEngine;

namespace Objects.Runtime
{
    public class ProjectileBehaviour : MonoBehaviour, IAmElement
    {
        #region Publics

        public IAmElement.Element spell => m_element;
        public IAmElement.Element m_element;

        public ParticleSystem m_particleImpact;

        private void Awake() => _timer = new(.5f);

        private void OnEnable()
        {
            _timer?.Reset();
            _timer.OnTimerFinished += Deactivate;
        }

        private void OnDisable() => _timer.OnTimerFinished -= Deactivate;

        private void Update() => _timer?.Tick();

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 6)
            {
                if (other.gameObject.TryGetComponent(out ICanBeHurt hurt))
                {
                    hurt.TakeDamage(_damages);
                }

                if(other.gameObject.TryGetComponent(out IAmElement element) && other.gameObject.TryGetComponent(out IAmObstacle obstacle))
                {
                    if(element.spell == m_element)
                    {
                        obstacle.ReactToSpell();
                    }
                }
            }

            if (other.gameObject.layer == 9)
            {
                other.TryGetComponent(out BossBehaviour bossBehaviour);
                bossBehaviour?.TakeDamage(_damages);
            }

            if (other.gameObject.layer == 9 || other.gameObject.layer == 6)
            {
                m_particleImpact.transform.position = gameObject.transform.position;
                m_particleImpact.Play();

                _timer?.Begin();
            }
        }

        private void Deactivate() => gameObject.SetActive(false);

        #endregion


        #region Private and Protected

        [SerializeField] private int _damages;
        private Timer _timer;

        #endregion
    }
}