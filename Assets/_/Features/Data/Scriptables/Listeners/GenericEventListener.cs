using UnityEngine;
using UnityEngine.Events;

namespace Data.Runtime
{
    public class GenericEventListener<T> : MonoBehaviour 
    {
        public UnityEvent<T> m_event;
        public GenericScriptableEvent<T> m_listerner;

        private void OnEnable()
        {
            m_listerner.Subscribe(this);
        }

        private void OnDisable()
        {
            m_listerner.Unsubscribe(this);
        }

        public void OnRaiseEvent(T _value) => m_event.Invoke(_value);
    }
}