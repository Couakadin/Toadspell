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
        public float m_jumpPlateform;
        [Required]
        public GroundBehaviour m_groundChecker;

        [Header("Cameras"), Required]
        public GameObject m_cameraTarget;

        [HideInInspector]
        public bool m_tonguePlateform;
        [HideInInspector]
        public Vector3 m_velocity;

        #endregion

        #region Unity

        private void Awake()
        {
            TryGetComponent(out _characterController);
            TryGetComponent(out _playerAnimator);

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

            _playerAnimator.SetFloat("VelocityX", _direction.x);
            _playerAnimator.SetFloat("VelocityZ", _direction.y);
            _playerAnimator.SetFloat("VelocityY", m_velocity.y);
            _playerAnimator.SetFloat("CurrentSpeed", _currentSpeed);
            _playerAnimator.SetFloat("SpeedMultiplier", Mathf.Abs(_direction.x * _direction.y) == 0 ? 2 : 3);
        }

        private void LateUpdate()
        {
            if (!m_tonguePlateform) _movement = new Vector3(_cameraDirection.x * _currentSpeed, m_velocity.y, _cameraDirection.z * _currentSpeed);
            else _movement = new Vector3(_characterController.transform.forward.x * m_jumpPlateform, m_velocity.y, _characterController.transform.forward.z * m_jumpPlateform);

            _playerBlackboard.SetValue("Position", transform.position);
            _playerBlackboard.SetValue("Contact", _pointOfContact.position);
        }

        #endregion

        #region Utils

        private void HandleMove()
        {
            if (m_groundChecker.m_isGrounded)
            {
                if (_jumpInput.triggered) m_velocity.y = Mathf.Sqrt(m_jump * -2f * m_gravity);
                else if (m_velocity.y < 0) m_velocity.y = 0;
                m_tonguePlateform = false;

                _playerAnimator.SetLayerWeight(0, 1f); // Move Layer
                _playerAnimator.SetLayerWeight(1, 0f); // Jump Layer
                _playerAnimator.SetBool("IsJump", false);
                return;
            }

            bool isFalling = m_velocity.y <= 0;
            float multiplier = isFalling ? m_fallMultiplier : m_jumpMultiplier;
            m_velocity.y += Time.deltaTime * m_gravity * multiplier;

            _playerAnimator.SetLayerWeight(0, 0f); // Move Layer
            _playerAnimator.SetLayerWeight(1, .7f); // Jump Layer
            _playerAnimator.SetBool("IsJump", true);
        }

        private void MoveAction()
        {
            // Handle rotation
            if (_direction.magnitude > .1f) RotateTowards(_direction);

            // Calculate movement
            _characterController.Move(Time.deltaTime * _movement);

            //_audioSource.PlayOneShot(_footsteps[_footstepsIndex]);
        }

        /// <summary>
        /// Updates the current speed by applying acceleration or deceleration.
        /// </summary>
        private void UpdateMovementSpeed()
        {
            float directionSqrMagnitude = _direction.sqrMagnitude;
            float targetSpeed = directionSqrMagnitude > 0 ? Mathf.Sqrt(directionSqrMagnitude) * m_speed : 0f;

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
                Vector3 newForward = Vector3.Lerp(_characterController.transform.forward, _cameraDirection.normalized, m_rotation);
                _characterController.transform.forward = newForward;
            }
        }

        #endregion

        #region Privates

        [Header("Blackboards"), Required]
        [SerializeField]
        private Blackboard _playerBlackboard;
        [SerializeField] private Transform _pointOfContact;

        private Animator _playerAnimator;

        private CharacterController _characterController;

        private GameInput _gameInput;
        private GameInput.GameplayActions _gameplayInput;
        private InputAction _moveInput;
        private InputAction _jumpInput;

        private Vector2 _direction;

        private Vector3 _cameraRight;
        private Vector3 _cameraForward;
        private Vector3 _cameraDirection;
        private Transform _cameraTransform;

        private float _currentSpeed;
        private Vector3 _movement;

        #endregion
    }
}