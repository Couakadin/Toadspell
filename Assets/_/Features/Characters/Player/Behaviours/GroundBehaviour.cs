using UnityEngine;

namespace Player.Runtime
{
    public class GroundBehaviour : MonoBehaviour
    {
        #region Publics

        public bool m_isGrounded { get; private set; }

        public GameObject m_particleParent;

        #endregion


        #region Unity

        private void Start()
        {
            m_camera = Camera.main;
            _particleFalling = m_particleParent.GetComponentsInChildren<ParticleSystem>();
        }

        private void Update()
        {
            // Check if there is any collider within the specified radius on the ground layer
            m_isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayer);

            if (m_isGrounded && !hasParticlePlayed)
            {
                PlayAllParticles();
                hasParticlePlayed = true; // Empêche de rejouer
            } else if (!m_isGrounded && hasParticlePlayed) hasParticlePlayed = false;
        }

        private void OnDrawGizmosSelected()
        {
            // Draw the sphere in the editor to visualize the ground check radius
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
        }

        #endregion

        #region Utils

        private void PlayAllParticles()
        {
            m_particleParent.transform.LookAt(m_camera.transform);

            foreach (ParticleSystem particleSystem in _particleFalling)
            {
                particleSystem.Play();
                Debug.Log(particleSystem.isPlaying);
            }
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

        private ParticleSystem[] _particleFalling;
        private bool hasParticlePlayed = false;

        private Camera m_camera;

        #endregion
    }
}
