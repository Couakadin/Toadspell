using UnityEngine;
using Data.Runtime;
using Sirenix.OdinInspector;

namespace Enemies.Runtime
{
    public class InteractBehaviour : MonoBehaviour, IAmInteractable
    {
        #region Publics

        public IAmInteractable.Size m_grapSize => _enemySize;

        public float m_offsetDistance => _offsetDistance;

        public IAmInteractable.SpellType spellType => _spellType;

        #endregion

        #region Privates

        [SerializeField, EnumToggleButtons]
        private IAmInteractable.Size _enemySize;
        [SerializeField, EnumToggleButtons]
        private IAmInteractable.SpellType _spellType;
        [SerializeField]
        private float _offsetDistance;

        #endregion
    }
}