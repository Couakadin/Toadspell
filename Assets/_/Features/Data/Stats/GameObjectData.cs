using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = "GameObjectTest", menuName = "Data/Scriptables/GameObject")]
    public class GameObjectData : ScriptableObject
    {
        #region Publics

        public GameObject gameObject;

        #endregion
    }
}