using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.Runtime
{
    public class ThornInteraction : MonoBehaviour
    {
        #region Main Methods

        public void ActivateThorns()
        {
            _thorns.SetActive(true);
        }

        #endregion

        #region Private & protected

        [SerializeField] private GameObject _thorns;

        #endregion
    }
}
