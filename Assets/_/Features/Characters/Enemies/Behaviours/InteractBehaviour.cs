using UnityEngine;

namespace Enemies.Runtime
{
    public class InteractBehaviour : MonoBehaviour
    {
        #region Publics

        public float m_offsetDistance => _offsetDistance;

        #endregion

        #region Privates
        
        [SerializeField]
        private float _offsetDistance;

        #endregion
    }
}