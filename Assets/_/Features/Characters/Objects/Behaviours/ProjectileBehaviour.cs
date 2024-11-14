using Data.Runtime;
using UnityEngine;

namespace Objects.Runtime
{
    public class ProjectileBehaviour : MonoBehaviour, IAmElement
    {
        #region Publics

        public IAmElement.Element spell => m_element;
        public IAmElement.Element m_element;


        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
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
            gameObject.SetActive(false);
        }

        #endregion


        #region Private and Protected

        [SerializeField] private int _damages;
        #endregion
    }
}