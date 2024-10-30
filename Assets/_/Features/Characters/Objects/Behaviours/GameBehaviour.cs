using Cinemachine;
using Data.Runtime;
using Player.Runtime;
using System;
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
            _teleportTimer = new Timer(_teleportDelay);
        }

        private void Start()
        {
            _player = Instantiate(m_player, m_position.position, Quaternion.identity, null);
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

        private void OnEnable()
        {
            _teleportTimer.OnTimerFinished += TeleportMove;
        }

        private void OnDisable()
        {
            _teleportTimer.OnTimerFinished -= TeleportMove;
        }

        private void Update()
        {
            _teleportTimer.Tick();
        }

        #region Main Methods

        private void TeleportMove()
        {
            _player.transform.position = _playerBlackboard.GetValue<Vector3>("Checkpoint");
            _player.SetActive(true);
        }

        #endregion

        [ContextMenu("disable")]
        public void DisablePlayer()
        {
            _teleportTimer.Reset();
            _teleportTimer.Begin();
            _player.SetActive(false);
        }

        #endregion

        #region Privates

        private GameObject _player;
        private PowerBehaviour _powerBehaviour;
        [SerializeField] private Blackboard _playerBlackboard;
        [SerializeField] private float _teleportDelay = 1f;
        private Timer _teleportTimer;

        #endregion
    }
}