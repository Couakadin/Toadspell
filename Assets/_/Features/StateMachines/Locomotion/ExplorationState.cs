using Data.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Runtime
{
    public class ExplorationState : AState, IAmStateMachine
    {
        #region Methods

        public ExplorationState(StateMachineCore stateMachine, GameInputObject inputReader, Dictionary<string, object> parameterDictionary = null) : base(stateMachine, parameterDictionary)
        { 
            this._stateMachine = stateMachine;
            this._inputReader = inputReader;
            this._parameterDictionary = parameterDictionary;

            // Get the forward and right directions based on the camera's orientation
            _cameraTransform = _mainCamera.transform;
        }

        public void Enter()
        {
            _thirdPersonCamera.Priority = 10;
        }

        public void Exit()
        {
            _thirdPersonCamera.Priority = 0;
        }

        public void Tick()
        {
            // Switch to AimState
            if (_playerBlackboard.GetValue<bool>("IsAiming"))
                _stateMachine.SetState(_aimState);

            // Switch to JumpState
            if (_inputReader.IsPerformed("Jump"))
                _stateMachine.SetState(_jumpState);
        }

        public void FixedTick()
        {
            Move(_playerBlackboard.GetValue<Vector2>("MoveDirection"));
        }

        public void LateTick()
        {
            _playerBlackboard.SetValue<Vector3>("Position", _playerTransform.position);
        }

        #endregion

        #region Utils

        // Movements
        /// <summary>
        /// Main method to move the player.
        /// </summary>
        /// <param name="moveDirection"></param>
        private void Move(Vector2 moveDirection)
        {
            // if there is no movement triggered, apply a deceleration
            if (moveDirection == Vector2.zero)
            {
                ApplyDeceleration();
                return;
            }

            // Forward direction relative to the camera
            _cameraForward = _cameraTransform.forward;
            _cameraRight = _cameraTransform.right;

            // Flatten the y-axis to ensure the player only moves along the ground
            _cameraForward.y = 0;
            _cameraRight.y = 0;

            // Normalize the vectors to ensure consistent movement speed
            _cameraForward.Normalize();
            _cameraRight.Normalize();

            // Convert the Vector2 moveDirection into a 3D direction relative to the camera
            _cameraDirection = (_cameraForward * moveDirection.y + _cameraRight * moveDirection.x);

            // Smoothly rotate the player towards the target direction
            SmoothRotateTowards(_cameraDirection);

            // Apply movement with acceleration
            ApplyMovement(_cameraDirection);
        }

        /// <summary>
        /// Method for a smooth rotation to the target direction.
        /// </summary>
        /// <param name="targetDirection"></param>
        private void SmoothRotateTowards(Vector3 targetDirection)
        {
            // Get the target rotation from the direction.
            _targetRotation = Quaternion.LookRotation(targetDirection);

            // Do rotate the player step by step to the target direction
            // With a Slerp interpolation.
            _playerRigidbody.rotation = Quaternion.Slerp(
                _playerRigidbody.rotation,
                _targetRotation,
                _playerStats.m_rotationSpeed * Time.fixedDeltaTime
            );
        }

        /// <summary>
        /// Apply a movement force with acceleration.
        /// </summary>
        /// <param name="direction"></param>
        private void ApplyMovement(Vector3 direction)
        {
            // Calculate the target velocity to the movement direction
            _targetVelocity = direction * _playerStats.m_moveSpeed;

            // Accelerate step by step to the target velocity
            // Using smoothDamp
            _playerRigidbody.velocity = Vector3.SmoothDamp(
                _playerRigidbody.velocity,
                _targetVelocity,
                ref _smoothVelocity,
                _playerStats.m_rotationTime,
                _playerStats.m_acceleration
            );
        }

        /// <summary>
        /// Apply a gradual deceleration when there is no movement triggered.
        /// </summary>
        private void ApplyDeceleration()
        {
            // When null velocity, decelerate the player
            _playerRigidbody.velocity = Vector3.Lerp(
                _playerRigidbody.velocity,
                Vector3.zero,
                _playerStats.m_deceleration * Time.fixedDeltaTime
            );
        }

        #endregion

        #region Privates

        // Physics
        private Vector3 _currentVelocity;
        private Vector3 _smoothVelocity;
        private Vector3 _targetVelocity;
        private Quaternion _targetRotation;

        // Cameras
        private Vector3 _cameraForward;
        private Vector3 _cameraRight;
        private Vector3 _cameraDirection;
        private Transform _cameraTransform;

        #endregion
    }
}
