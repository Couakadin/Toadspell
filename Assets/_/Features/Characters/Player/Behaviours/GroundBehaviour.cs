using UnityEngine;

namespace Player.Runtime
{
    public class GroundBehaviour : MonoBehaviour
    {
        #region Publics

        public bool m_isGrounded { get; private set; }

        #endregion

        #region Unity
        private void Update()
        {
            // Check if there is any collider within the specified radius on the ground layer
            m_isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayer);
        }

        private void OnDrawGizmosSelected()
        {
            // Draw the sphere in the editor to visualize the ground check radius
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
        }

        #endregion

        #region Privates

        [Header("Ground Detection Settings")]
        [Tooltip("The radius of the sphere used to check for ground.")]
        [SerializeField]
        private float groundCheckRadius = .2f;

        [Tooltip("Layer(s) considered as ground.")]
        [SerializeField]
        private LayerMask groundLayer;

        #endregion
    }
}
