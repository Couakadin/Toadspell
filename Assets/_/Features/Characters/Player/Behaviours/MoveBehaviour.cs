using Data.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    [RequireComponent(typeof(CharacterController))]
    public class MoveBehaviour : MonoBehaviour
    {
        #region Publics

        [Header("Move Params")]
        public float m_speed;
        public float m_rotation;
        public float m_speedAcceleration;
        public float m_speedDeceleration;

        [Header("Jump Params")]
        public float m_jump;
        public float m_gravity;
        public float m_fallMultiplier;
        public float m_jumpMultiplier;
        [Required]
        public GroundBehaviour m_groundChecker;

        [Header("Cameras"), Required]
        public GameObject m_cameraTarget;

        #endregion

        #region Unity

        private void Awake()
        {
            TryGetComponent(out _characterController);

            _gameInput = new GameInput();
            _gameplayInput = _gameInput.Gameplay;
            _moveInput = _gameplayInput.Move;
            _jumpInput = _gameplayInput.Jump;

            _cameraTransform = Camera.main.transform;
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
            _direction = _moveInput.ReadValue<Vector2>();

            UpdateMovementSpeed();
            HandleMove();
            MoveAction();
        }

        private void LateUpdate()
        {
            _playerBlackboard.SetValue<Vector3>("Position", transform.position);
        }

        #endregion

        #region Methods

        public void JumpTrigger()
        {
            _velocity.y = Mathf.Sqrt(m_jump * -2f * m_gravity);
            Vector3 movement = new Vector3(_cameraDirection.x * _velocity.y * _currentSpeed, _velocity.y, _cameraDirection.z * _velocity.y * _currentSpeed);

            _characterController.Move(Time.deltaTime * movement);
        }

        #endregion

        #region Utils

        private void HandleMove()
        {
            if (m_groundChecker.m_isGrounded)
            {
                if (_jumpInput.triggered)
                {
                    // trigger jump
                    _velocity.y = Mathf.Sqrt(m_jump * -2f * m_gravity);
                }
                else if (_velocity.y <= 0)
                {
                    _velocity.y = 0;
                }
                return;
            }

            bool isFalling = _velocity.y <= 0;
            float multiplier = isFalling ? m_fallMultiplier : m_jumpMultiplier;
            _velocity.y += Time.deltaTime * m_gravity * multiplier;
        }

        private void MoveAction()
        {
            // Handle rotation
            if (_direction.magnitude > 0.1f) RotateTowards(_direction);

            // Calculate movement
            Vector3 movement = new Vector3(_cameraDirection.x * _currentSpeed, _velocity.y, _cameraDirection.z * _currentSpeed);
            _characterController.Move(Time.deltaTime * movement);
        }

        /// <summary>
        /// Updates the current speed by applying acceleration or deceleration.
        /// </summary>
        private void UpdateMovementSpeed()
        {
            float targetSpeed = _direction.magnitude > 0 ? m_speed : 0f;

            if (targetSpeed > _currentSpeed)
            {
                _currentSpeed += Time.deltaTime * m_speedAcceleration;
                _currentSpeed = Mathf.Min(_currentSpeed, targetSpeed);
            }
            else if (targetSpeed < _currentSpeed)
            {
                _currentSpeed -= Time.deltaTime * m_speedDeceleration;
                _currentSpeed = Mathf.Max(_currentSpeed, targetSpeed);
            }
        }

        /// <summary>
        /// Method for a smooth rotation to the target direction.
        /// </summary>
        /// <param name="direction"></param>
        private void RotateTowards(Vector3 direction)
        {
            _cameraForward = _cameraTransform.forward;
            _cameraRight = _cameraTransform.right;

            _cameraForward.y = 0;
            _cameraRight.y = 0;

            _cameraForward.Normalize();
            _cameraRight.Normalize();

            _cameraDirection = (_cameraForward * direction.y + _cameraRight * direction.x);

            if (_cameraDirection.magnitude > 0.1f)
            {
                Vector3 newForward = Vector3.Lerp(_characterController.transform.forward, _cameraDirection.normalized, Time.deltaTime * m_rotation);
                _characterController.transform.forward = newForward;
            }
        }

        #endregion

        #region Privates

        [Header("Blackboards"), Required]
        [SerializeField]
        private Blackboard _playerBlackboard;

        private CharacterController _characterController;

        private GameInput _gameInput;
        private GameInput.GameplayActions _gameplayInput;
        private InputAction _moveInput;
        private InputAction _jumpInput;

        private Vector3 _velocity;
        private Vector2 _direction;

        private Vector3 _cameraRight;
        private Vector3 _cameraForward;
        private Vector3 _cameraDirection;
        private Transform _cameraTransform;

        private bool _isGrounded;

        private float _currentSpeed;

        #endregion
    }
}