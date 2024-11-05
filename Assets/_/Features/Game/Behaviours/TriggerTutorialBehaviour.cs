using Data.Runtime;
using UnityEngine;

namespace Game.Runtime
{
    public class TriggerTutorialBehaviour : MonoBehaviour
    {
        #region Unity API

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
                _onTutorialTriggeredVoidEVent.Raise();
                gameObject.SetActive(false);
            }
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private VoidEvent _onTutorialTriggeredVoidEVent;

        #endregion
    }
}