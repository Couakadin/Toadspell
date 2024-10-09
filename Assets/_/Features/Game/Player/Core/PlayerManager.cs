using Data.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.Runtime
{
    public class PlayerManager : MonoBehaviour
    {
        #region Unity

        private void Awake()
        {
            _speed = _playerBlackboard.GetValue<float>("Speed");
            _jumpSpeed = _playerBlackboard.GetValue<float>("JumpSpeed");
        }

        private void OnEnable()
        {
            _inputReader.MoveEvent += HandleMove;

            _inputReader.JumpEvent += HandleJump;
            _inputReader.JumpCancelledEvent += HandleCancelledJump;

            _inputReader.AimEvent += HandleAim;
        }

        private void OnDisable()
        {
            _inputReader.MoveEvent -= HandleMove;

            _inputReader.JumpEvent -= HandleJump;
            _inputReader.JumpCancelledEvent -= HandleCancelledJump;

            _inputReader.AimEvent -= HandleAim;
        }

        private void FixedUpdate()
        {
            Move();
            Jump();
            Aim();
        }

        #endregion

        #region Utils

        private void HandleMove(Vector2 dir) => _moveDirection = dir;

        private void HandleJump() => _isJumping = true;
        private void HandleCancelledJump() => _isJumping = false;

        private void HandleAim() => _isAiming = _isAiming ? false : true;

        private void Move()
        {
            if (_moveDirection == Vector2.zero) return;
            transform.position += new Vector3(_moveDirection.x, 0, _moveDirection.y) * (_speed * Time.fixedDeltaTime);
        }

        private void Jump()
        {
            if (_isJumping)
                transform.position += new Vector3(0, 1, 0) * (_jumpSpeed * Time.fixedDeltaTime);
        }

        private void Aim()
        {
            if (_isAiming)
            {
                _moveDirection = Vector2.zero;
                _isJumping = false;
            }
        }

        #endregion

        #region Privates

        [Title("Blackboard")]
        [SerializeField]
        private Blackboard _playerBlackboard;

        [Title("Input")]
        [SerializeField]
        private InputReader _inputReader;

        [Title("Privates")]
        private Vector2 _moveDirection;

        private float _speed, _jumpSpeed;

        private bool _isJumping, _isAiming;

        #endregion
    }
}
