using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = "PlayerStatsData", menuName = "Data/Player Stats")]

    public class PlayerStatsData : ScriptableObject
    {
        #region Publics

        // Movement Stats
        [InfoBox("Movement Stats (Cursor on properties to show how they work)")]
        [Tooltip("Speed of the character")]
        public float m_moveSpeed;
        [Tooltip("Speed of rotation when changing direction")]
        public float m_rotationSpeed;
        [Tooltip("Time between the old and new position when rotate")]
        public float m_rotationTime;
        [Tooltip("Acceleration when starting move")]
        public float m_acceleration;
        [Tooltip("Deceleration when stopping move")]
        public float m_deceleration;
        [Tooltip("Gravity settings")]
        public float m_jumpGravityMultiplier, m_fallGravityMultiplier;

        // Jump Stats
        [InfoBox( "Jump Stats (Cursor on properties to show how they work)")]
        [Tooltip("The force of a jump")]
        public float m_jumpForce;
        [Tooltip("The speed movement of the character is in the air")]
        public float m_airControlSpeed;
        [Tooltip("The speed rotation of the character is in the air")]
        public float m_airControlRotate;

        // Lock Stats
        [InfoBox("Lock Stats (Cursor on properties to show how they work)")]
        [Tooltip("The field distance to lock something")]
        public float m_detectionRadius;
        [Tooltip("Layer to limit detection to specific layers (optional)")]
        public LayerMask m_detectionLayer;

        #endregion
    }
}
