using Cinemachine;
using Data.Runtime;
using Player.Runtime;
using Enemies.Runtime;
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

        [Header("Init Boss")]
        [Tooltip("The Boss Prefab to init.")]
        public BossBehaviour m_boss;

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
            _playerBlackboard.SetValue<int>("Lives", _startLifePoints);
            LockCursor();
            _teleportTimer = new Timer(_teleportDelay);
            _disablingTimer = new Timer(_disablingDelay);

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

            m_boss.m_player = _player;

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
            DisableGameplayInput();
        }

        public void onStartDiscoveringTheWorld()
        {
            EnableGameplayInput();
        }

        public void DisableGameplayInput() 
        {
            _moveBehaviour.DeactivateInputSystem();
            _powerBehaviour.DeactivateInputSystem();
        }

        public void EnableGameplayInput()
        {
            _moveBehaviour.ActivateInputSystem();
            _powerBehaviour.ActivateInputSystem();
        }


        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void UnLockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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