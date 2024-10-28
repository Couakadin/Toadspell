using Cinemachine;
using UnityEngine;

namespace Objects.Runtime
{
    public class GameBehaviour : MonoBehaviour
    {
        #region Publics

        [Header("Init Player")]
        [Tooltip("The Player Prefab to init.")]
        public GameObject m_player;
        [Tooltip("The init position to spawn the Player.")]
        public Transform m_position;

        [Header("Init Camera")]
        [Tooltip("The Third Person Camere to init.")]
        public CinemachineVirtualCamera m_camera;

        #endregion

        #region Unity

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Start()
        {
            _player = Instantiate(m_player, m_position);

            m_camera.Follow = _player.transform;
            m_camera.LookAt = _player.transform;
        }

        #endregion

        #region Privates

        private GameObject _player;

        #endregion
    }
}