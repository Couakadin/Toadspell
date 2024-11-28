using Data.Runtime;
using UnityEngine;

namespace Enemies.Runtime
{
    public class ThrowProjectileBehaviour : MonoBehaviour
    {
        #region Publics

        public PoolSystem m_projectilePool;

        #endregion


        #region Main Methods

        public void ShootOnTime()
        {
            GameObject projectile = m_projectilePool.GetFirstAvailableObject();
            projectile.transform.position = _mouth.position;
            projectile.transform.rotation = _mouth.rotation;
            projectile.transform.forward = _mouth.forward;
            _enemySoundBehaviour.PlaySoundWhenAttacking();
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private Transform _mouth;
        [SerializeField] private EnemySoundBehaviour _enemySoundBehaviour;
        #endregion
    }
}