using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = "PoolReference", menuName = "Data/Scriptables/Lists/PoolSystem")]
    public class PoolReferenceData : ScriptableObject
    {
        #region Publics

        public PoolSystem poolSystem;
        
        #endregion
    }
}