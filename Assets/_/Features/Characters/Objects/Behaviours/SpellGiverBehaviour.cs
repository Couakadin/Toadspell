using Data.Runtime;
using Player.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Objects.Runtime
{
    public class SpellGiverBehaviour : MonoBehaviour
    {
        #region Unity

        private void Awake()
        {
            _initialPosition = transform.position;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out PowerBehaviour interact))
            {
                interact.m_spell = _spell;
                Invoke(nameof(RespawnAfterAWhile), 3);
                gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            transform.position = _initialPosition;
        }

        #endregion

        #region Methods

        public IAmSpellGiver.Spell spell => _spell;

        #endregion

        #region Utils

        private void RespawnAfterAWhile() => gameObject.SetActive(true);

        #endregion

        #region Privates

        [SerializeField, EnumToggleButtons]
        private IAmSpellGiver.Spell _spell;

        private Vector3 _initialPosition;

        #endregion
    }
}
