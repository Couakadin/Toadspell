using Data.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class TriggerDialogueBehaviour : MonoBehaviour
    {
        #region Unity API

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                _onDialogueTriggeredVoidEvent.Raise();
                gameObject.SetActive(false);
            }
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private VoidEvent _onDialogueTriggeredVoidEvent;

        #endregion
    }
}