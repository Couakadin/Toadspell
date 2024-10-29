using Cinemachine;
using Data.Runtime;
using Player.Runtime;
using System.Collections.Generic;
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

        [Header("Init Pools")]
        [Tooltip("The spell pool to init.")]
        public List<PoolSystem> m_spellPools;

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
            _player.TryGetComponent(out _powerBehaviour);

            m_camera.Follow = _player.transform;
            m_camera.LookAt = _player.transform;
            m_camera.transform.forward = _player.transform.forward;

            foreach (PoolSystem pool in m_spellPools)
            {
                pool.m_poolTransform = _player.transform;
                _powerBehaviour.m_spellPools.Add(pool);
            }
        }

        #endregion

        #region Privates

        private GameObject _player;
        private PowerBehaviour _powerBehaviour;

        #endregion
    }
}