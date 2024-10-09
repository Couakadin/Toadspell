using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    public class PlayerManager : MonoBehaviour
    {
        #region Unity

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _movementAction = _playerInput.actions["Movement"];
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _playerInput.ActivateInput();
        }

        private void OnDisable()
        {
            _playerInput.DeactivateInput();
        }

        private void FixedUpdate()
        {
            Move();
        }

        #endregion

        #region Utils

        private void Move()
        {
            Vector2 position = _movementAction.ReadValue<Vector2>();

            _horizontal = position.x;
            _vertical = position.y;

            _rigidbody.velocity = new Vector3(
                _horizontal * _movementSpeed * Time.fixedDeltaTime,
                _rigidbody.velocity.y,
                _vertical * _movementSpeed * Time.fixedDeltaTime);
        }

        #endregion

        #region Privates

        private PlayerInput _playerInput;
        private InputAction _movementAction;

        [Title("Privates")]
        private float _movement;
        private Rigidbody _rigidbody;
        private LayerMask _groundLayer;
        private Vector3 _forward, _right;

        private float _movementSpeed = 5f;
        private float _horizontal, _vertical;

        #endregion
    }
}
