using System.Collections.Generic;
using UnityEngine;

namespace Data.Runtime
{
    public class GenericScriptableEvent<T> : ScriptableObject
    {
        private List<GenericEventListener<T>> _list = new List<GenericEventListener<T>>();

        public void Subscribe(GenericEventListener<T> _event)
        {
            _list.Add(_event);
        }

        public void Unsubscribe(GenericEventListener<T> _event)
        {
            _list.Remove(_event);
        }

        public void Raise(T _value)
        {
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                _list[i].OnRaiseEvent(_value);
            }
        }
    }
}