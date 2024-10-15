using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = "TongueStats", menuName = "Data/Scriptables/TongueStats")]
    public class TongueStatsData : ScriptableObject
    {
        #region Publics

        [InfoBox("Tongue Power (Cursor on properties to show how they work)")]
        [Tooltip("Max distance of the tongue in AimState")]
        public float m_maxDistanceAim;
        [Tooltip("Max distance of the tongue in ExplorationState")]
        public float m_maxDistanceLock;
        [Tooltip("Speed of the tongue")]
        public float m_speed;
        [Tooltip("The final position behind the target")]
        public float m_offsetDistance;
        [Tooltip("The speed the player turns to target")]
        public float m_rotationSpeed;

        #endregion
    }
}