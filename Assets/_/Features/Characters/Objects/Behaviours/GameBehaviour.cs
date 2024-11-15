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
        public CinemachineVirtualCamera m_frontCamera;

        [Header("Init Pools")]
        [Tooltip("The spell pool to init.")]
        public List<PoolSystem> m_spellPools;

        #endregion


        #region Unity

        private void Awake()
        {
            _playerBlackboard.SetValue<int>("Lives", _startLifePoints);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _teleportTimer = new Timer(_teleportDelay);
            _disablingTimer = new Timer(_disablingDelay);
            m_frontCamera.Priority = 11;
        }

        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new();
            }

            _teleportTimer.OnTimerFinished += TeleportMove;
            _disablingTimer.OnTimerFinished += DisablePlayer;
        }

        private void OnDisable()
        {
            _teleportTimer.OnTimerFinished -= TeleportMove;
            _disablingTimer.OnTimerFinished -= DisablePlayer;
        }

        private void Start()
        {
            _player = Instantiate(m_player, m_position.position, m_position.rotation, null);
            _player.SetActive(true);
            _player.TryGetComponent(out _powerBehaviour);
            _player.TryGetComponent(out _moveBehaviour);
            _player.TryGetComponent(out _healthBehaviour);

            _healthBehaviour.m_startLives = _startLifePoints;

            m_camera.Follow = _moveBehaviour.m_cameraTarget.transform;
            m_camera.LookAt = _moveBehaviour.m_cameraTarget.transform;
            m_camera.transform.forward = _moveBehaviour.m_cameraTarget.transform.forward;

            foreach (PoolSystem pool in m_spellPools)
            {
                pool.m_poolTransform = _player.transform;
                _powerBehaviour.m_spellPools.Add(pool);
            }
        }

        private void Update()
        {
            _teleportTimer?.Tick();
            _disablingTimer?.Tick();
        }

        #endregion


        #region Main Methods

        public void ReactToTeleportEvent()
        {
            SetOrResetTimer(_teleportTimer);
            SetOrResetTimer(_disablingTimer);
        }

        [ContextMenu("disabled input")]
        public void onFirstSpawnDontMove()
        {
            _gameInput.Gameplay.Disable();
            _gameInput.Dialogue.Enable();
        }

        public void onStartDiscoveringTheWorld()
        {
            m_frontCamera.Priority = 0;
            _gameInput.Gameplay.Enable();
            _gameInput.Dialogue.Disable();
        }
        #endregion


        #region Utils
        private void TeleportMove()
        {
            Debug.Log("TELEPORT");
            _player.transform.position = _playerBlackboard.GetValue<Vector3>("Checkpoint");
            _player.SetActive(true);
        }

        public void DisablePlayer()
        {
            _player.SetActive(false);
        }

        public void SetOrResetTimer(Timer timer)
        {
            if (!timer.IsRunning())
            {
                timer.Reset();
                timer.Begin();
            }
        }

        #endregion

        #region Privates

        private GameObject _player;
        private PowerBehaviour _powerBehaviour;
        private MoveBehaviour _moveBehaviour;
        private HealthBehaviour _healthBehaviour;
        [SerializeField] private Blackboard _playerBlackboard;
        [SerializeField] private float _teleportDelay = 1f;
        [SerializeField] private float _disablingDelay = .5f;
        [SerializeField] private int _startLifePoints = 4;
        private Timer _teleportTimer;
        private Timer _disablingTimer;
        private GameInput _gameInput;

        #endregion
    }
}