using Data.Runtime;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    public class PowerBehaviour : MonoBehaviour, IAmElement
    {
        #region Publics

        [Header("Blackboards")]
        public Blackboard _playerBlackboard;
        public Blackboard m_tongueBlackboard;

        [Header("Lock Params")]
        [Tooltip("The max range to detect lockable targets.")]
        public float m_maxDetectionRadius;
        [Tooltip("The min range to detect lockable targets.")]
        public float m_minDetectionRadius;
        [Tooltip("The layer of lockable targets.")]
        public LayerMask m_detectionLayer;

        [Header("Tongue Params")]
        [Tooltip("The GameObject of the tongue.")]
        public GameObject m_tongue;
        [Tooltip("The mesh of the tongue.")]
        public Transform m_tongueMesh;
        [Tooltip("Initial position of the tongue Mesh.")]
        public Transform m_tongueMeshPosition;
        [Tooltip("The speed of the tongue.")]
        public float m_tongueSpeed;
        [Tooltip("Bool if frog can eat this frame."), HideInInspector]
        public bool m_canEat;

        [Header("Element Params")]
        [Tooltip("The GameObject where spells are launched.")]
        public GameObject m_spellSpawner;
        [Tooltip("The speed at spells are launched.")]
        public float m_speedOfProjectile;
        [Tooltip("The duration of spells.")]
        public float m_durationOfProjectile;
        [EnumToggleButtons]
        public IAmElement.Element m_spell;
        public PoolSystem m_currentPool;
        public List<PoolSystem> m_spellPools;

        [Header("Parry Params")]
        [Tooltip("Timer of parrying.")]
        public float m_durationOfParry;
        [Tooltip("The GameObject of parrying.")]
        public GameObject m_parryObject;

        public InputAction m_lockInput { get; private set; }
        public InputAction m_tongueInput { get; private set; }
        public InputAction m_moveInput { get; private set; }
        public InputAction m_spellInput { get; private set; }
        public InputAction m_parryInput { get; private set; }

        public Animator m_playerAnimator { get; private set; }

        #endregion


        #region Unity

        private void Awake()
        {
            _gameInput = new GameInput();
            _gameplayInput = _gameInput.Gameplay;
            m_lockInput = _gameplayInput.Lock;
            m_tongueInput = _gameplayInput.Tongue;
            m_moveInput = _gameplayInput.Move;
            m_spellInput = _gameplayInput.Spell;
            m_parryInput = _gameplayInput.Parry;

            TryGetComponent(out _animator);
            m_playerAnimator = _animator;
        }

        private void Start()
        {
            _stateMachine = new(this);
            _stateMachine.ChangeState(_stateMachine.m_lockState);

            m_spell = IAmElement.Element.arcane;
        }

        private void OnEnable()
        {
            _gameInput.Enable();
        }

        private void OnDisable()
        {
            _gameInput.Disable();
        }

        private void Update()
        {
            _stateMachine.HandleInput();
            _stateMachine.Tick();

            switch(spell)
            {
                case (IAmElement.Element.arcane):
                    m_currentPool = m_spellPools.Find(spell => spell.gameObject.name == "Arcane Pool");
                    break;
                case (IAmElement.Element.fire):
                    m_currentPool = m_spellPools.Find(spell => spell.gameObject.name == "Fire Pool");
                    break;
                case (IAmElement.Element.water):
                    m_currentPool = m_spellPools.Find(spell => spell.gameObject.name == "Water Pool");
                    break;
                case (IAmElement.Element.grass):
                    m_currentPool = m_spellPools.Find(spell => spell.gameObject.name == "Grass Pool");
                    break;
            }
        }

        private void FixedUpdate()
        {
            _stateMachine.PhysicsTick();

            m_tongueMesh.transform.position = m_tongueMeshPosition.position;
        }

        private void LateUpdate()
        {
            _stateMachine.FinalTick();
            _playerBlackboard.SetValue("SpellPosition", m_spellSpawner.transform.position);
        }

        #endregion


        #region Methods

        public IAmElement.Element spell => m_spell;

        public void CastASpell()
        {
            int spellType = (int)m_spell;
            _playerSoundBehaviour.PlaySpellSound(spellType);
        }

        public void TongueSoundOnExtension()
        {
            _playerSoundBehaviour.PlayTongueSound();
        }
        #endregion


        #region Privates

        private StateMachine _stateMachine;

        private Animator _animator;

        private GameInput _gameInput;
        private GameInput.GameplayActions _gameplayInput;
        [SerializeField] PlayerSoundBehaviour _playerSoundBehaviour;

        #endregion
    }
}