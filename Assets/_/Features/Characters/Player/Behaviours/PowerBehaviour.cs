using Data.Runtime;
using Sirenix.OdinInspector;
using StateMachine.Runtime;
using System.Collections.Generic;
using UnityEngine;
using static Data.Runtime.IAmSpellGiver;

namespace Player.Runtime
{
    public class PowerBehaviour : MonoBehaviour, IAmSpellGiver
    {
        #region Publics

        [EnumToggleButtons]
        public Spell spell => m_spell;

        public Spell m_spell;

        #endregion

        #region Unity

        private void Awake()
        {
            // Components
            TryGetComponent<Transform>(out _transform);

            // Dictionary
            _sharedParameterDictionary.Add("transform", _transform);
            _sharedParameterDictionary.Add("playerBlackboard", _playerBlackboard);
            _sharedParameterDictionary.Add("tongueBlackboard", _tongueBlackboard);
            _sharedParameterDictionary.Add("playerStats", _playerStats);
            _sharedParameterDictionary.Add("tongueStats", _tongueStats);

            m_spell = Spell.arcane;
            if (_currentPool == null) _currentPool = _arcanePool;
        }

        private void Start()
        {
            // StateMachine
            _stateMachine = new();
            PowerlessState powerlessState = new(_stateMachine, _inputReader, _sharedParameterDictionary);
            TongueState tongueState = new(_stateMachine, _inputReader, _tongueTip, _sharedParameterDictionary);
            SpellState spellState = new(_stateMachine, _sharedParameterDictionary);

            tongueState.SetPowerlessState(powerlessState);
            spellState.SetPowerlessState(powerlessState);
            powerlessState.SetTongueState(tongueState);
            powerlessState.SetSpellState(spellState);

            // Initial State
            _stateMachine.SetState(powerlessState);
        }

        private void Update()
        {
            _stateMachine.Tick();
           
            UpdateCurrentPool();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedTick();
        }

        private void LateUpdate()
        {
            _stateMachine.LateTick();
            _tongueBlackboard.SetValue<Vector3>("TonguePosition", _tongueTip.transform.position);
            _playerBlackboard.SetValue<PoolSystem>("CurrentPool", _currentPool);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IAmSpellGiver interact))
            {
                m_spell = interact.spell;
                collision.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Utils

        /// <summary>
        /// Updates the current active pool based on the selected spell.
        /// </summary>
        private void UpdateCurrentPool()
        {
            if (_currentSpell == spell) return;
            _currentSpell = spell;

            switch (spell)
            {
                case Spell.fire:
                    _currentPool = _firePool;
                    break;
                case Spell.water:
                    _currentPool = _waterPool;
                    break;
                case Spell.grass:
                    _currentPool = _grassPool;
                    break;
                case Spell.arcane:
                    _currentPool = _arcanePool;
                    break;
            }
        }

        #endregion

        #region Privates

        // Dictionary
        private Dictionary<string, object> _sharedParameterDictionary = new();

        // StateMachine
        private StateMachineCore _stateMachine;

        // Components
        private Transform _transform;

        // Input
        [Title("ScriptableObjects")]
        [SerializeField]
        private GameInputObject _inputReader;
        // Blackboard
        [SerializeField]
        private Blackboard _playerBlackboard;
        [SerializeField]
        private Blackboard _tongueBlackboard;
        // Base Stats
        [SerializeField]
        private PlayerStatsData _playerStats;
        [SerializeField]
        private TongueStatsData _tongueStats;

        // GameObjects
        [Title("Objects")]
        [SerializeField]
        private GameObject _tongueTip;

        // Pools
        [Title("Pools")]
        [SerializeField]
        private PoolSystem _firePool;
        [SerializeField]
        private PoolSystem _waterPool;
        [SerializeField]
        private PoolSystem _grassPool;
        [SerializeField]
        private PoolSystem _arcanePool;
        [SerializeField]
        private PoolSystem _currentPool;
        private Spell _currentSpell;

        #endregion
    }
}
