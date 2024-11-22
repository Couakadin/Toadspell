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
                        Debug.Log("test");
                        obstacle.ReactToSpell();
                    }
                }
            }

            if (other.gameObject.layer == 9)
            {
                other.TryGetComponent(out BossBehaviour bossBehaviour);
                bossBehaviour?.TakeDamage(_damages);
            }
            gameObject.SetActive(false);
        }

        #endregion


        #region Private and Protected

        [SerializeField] private int _damages;
        #endregion
    }
}