using UnityEngine;
using UnityEngine.Events;

namespace Data.Runtime
{
    public class VoidEventListener : MonoBehaviour
    {
        public UnityEvent m_event;
        public VoidEvent m_listerner;

        private void OnEnable()
        {
            m_listerner.Subscribe(this);
        }

        private void OnDisable()
        {
            m_listerner.Unsubscribe(this);
        }

        public void OnRaiseEvent() => m_event.Invoke();
    }
}