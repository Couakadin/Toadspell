using Data.Runtime;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    public class PowerBehaviour : MonoBehaviour, IAmSpellGiver
    {
        #region Publics

        [Header("Blackboards")]
        public Blackboard _playerBlackboard;
        public Blackboard m_tongueBlackboard;

        [Header("Lock Params")]
        [Tooltip("The range to detect lockable targets.")]
        public float m_detectionRadius;
        [Tooltip("The layer of lockable targets.")]
        public LayerMask m_detectionLayer;

        [Header("Tongue Params")]
        [Tooltip("The GameObject of the tongue.")]
        public GameObject m_tongue;
        [Tooltip("The speed of the tongue.")]
        public float m_tongueSpeed;

        public InputAction m_lockInput { get; private set; }
        public InputAction m_tongueInput { get; private set; }
        public InputAction m_moveInput { get; private set; }
        public InputAction m_spellInput { get; private set; }

        [Header("Spell Params")]
        [Tooltip("The GameObject where spells are launched.")]
        public GameObject m_spellSpawner;
        [EnumToggleButtons]
        public IAmSpellGiver.Spell m_spell;
        public PoolSystem m_currentPool;
        public List<PoolSystem> m_spellPools;

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
        }

        private void Start()
        {
            _stateMachine = new(this);
            _stateMachine.ChangeState(_stateMachine.m_lockState);

            m_spell = IAmSpellGiver.Spell.arcane;
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
                case (IAmSpellGiver.Spell.arcane):
                    m_currentPool = m_spellPools.Find(spell => spell.gameObject.name == "Arcane Pool");
                    break;
                case (IAmSpellGiver.Spell.fire):
                    m_currentPool = m_spellPools.Find(spell => spell.gameObject.name == "Fire Pool");
                    break;
                case (IAmSpellGiver.Spell.water):
                    m_currentPool = m_spellPools.Find(spell => spell.gameObject.name == "Water Pool");
                    break;
                case (IAmSpellGiver.Spell.grass):
                    m_currentPool = m_spellPools.Find(spell => spell.gameObject.name == "Grass Pool");
                    break;
            }
        }

        private void FixedUpdate()
        {
            _stateMachine.PhysicsTick();
        }

        private void LateUpdate()
        {
            _stateMachine.FinalTick();
            _playerBlackboard.SetValue("SpellPosition", m_spellSpawner.transform.position);
        }

        #endregion

        #region Methods

        public IAmSpellGiver.Spell spell => m_spell;

        #endregion

        #region Privates

        private StateMachine _stateMachine;

        private GameInput _gameInput;
        private GameInput.GameplayActions _gameplayInput;

        #endregion
    }
}