using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = "Blackboard", menuName = "Data/Blackboard")]
    public class Blackboard : SerializedScriptableObject
    {
        #region Methods

        public T GetValue<T>(string key) => _data.TryGetValue(key, out var value) ? (T)value : default;
        public void SetValue<T>(string key, T value) => _data[key] = value;
        public bool ContainsKey(string key) => _data.ContainsKey(key);

        #endregion

        #region Privates

        [SerializeField]
        private Dictionary<string, object> _data = new();

        #endregion
    }
}
