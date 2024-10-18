using System;
using UnityEngine;

namespace Data.Runtime
{
    public class EventBus : MonoBehaviour
    {
        #region Publics

        public event Action m_onTeleport;

        #endregion


        #region Main Methods

        public void OnTeleportEventRaise() 
        {
            m_onTeleport.Invoke();
        }

        #endregion

    }
}