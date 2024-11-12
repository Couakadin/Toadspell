using Data.Runtime;
using System.Collections;
using System.Collections.Generic;
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
            projectile.SetActive(true);
            projectile.transform.position = _mouth.position;
            projectile.transform.rotation = _mouth.rotation;
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private Transform _mouth;

        #endregion
    }
}