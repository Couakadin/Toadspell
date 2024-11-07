using Data.Runtime;
using Player.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Objects.Runtime
{
    [RequireComponent(typeof(FixedJoint))]
    public class SpellBehaviour : MonoBehaviour
    {
        #region Unity

        private void Awake()
        {
            _initialPosition = transform.position;
            TryGetComponent(out _initialJoint);
            _initialJoint.anchor = _initialPosition;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out PowerBehaviour interact))
            {
                if (!interact.m_canEat) return;

                interact.m_spell = _spell;
                _onSpellChange.Raise((int)_spell);
                Invoke(nameof(RespawnAfterAWhile), 3);
                gameObject.SetActive(false);
                interact.m_canEat = false;
            }
        }

        private void OnEnable()
        {
            transform.position = _initialPosition;
            _initialJoint.anchor = _initialPosition;
        }

        #endregion

        #region Methods

        public IAmElement.Element spell => _spell;

        #endregion

        #region Utils

        private void RespawnAfterAWhile() => gameObject.SetActive(true);

        #endregion

        #region Privates

        [SerializeField, EnumToggleButtons]
        private IAmElement.Element _spell;

        [SerializeField]
        private IntEvent _onSpellChange;

        private Vector3 _initialPosition;
        private FixedJoint _initialJoint;

        #endregion
    }
}
