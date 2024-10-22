using System.Collections.Generic;
using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = nameof(MyEvent), menuName = Consts.SCRIPTABLES_SUBMENU + nameof(MyEvent))]
    public class VoidEvent : ScriptableObject
    {
        private List<VoidEventListener> _list = new List<VoidEventListener>();

        public void Subscribe(VoidEventListener _event)
        {
            _list.Add(_event);
        }

        public void Unsubscribe(VoidEventListener _event)
        {
            _list.Remove(_event);
        }

        public void Raise()
        {
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                _list[i].OnRaiseEvent();
            }
        }
    }

}