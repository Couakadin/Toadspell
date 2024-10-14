using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = "TongueStats", menuName = "Data/Scriptables/TongueStats")]
    public class TongueStatsData : ScriptableObject
    {
        #region Publics

        [InfoBox("Tongue Power (Cursor on properties to show how they work)")]
        [Tooltip("Max distance of the tongue")]
        public float m_maxDistance;
        [Tooltip("Speed of the tongue")]
        public float m_speed;

        #endregion
    }
}