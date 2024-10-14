using Data.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Runtime
{
    public class JumpState : AState, IAmStateMachine
    {
        #region Methods

        public JumpState(StateMachineCore stateMachine, Dictionary<string, object> parameterDictionary = null) : base(stateMachine, parameterDictionary)
        {
            this._stateMachine = stateMachine;
            this._parameterDictionary = parameterDictionary;

            // Get the forward and right directions based on the camera's orientation
            _cameraTransform = _mainCamera.transform;
        }

        public void Enter()
        {
            Jump(_playerBlackboard.GetValue<Vector2>("MoveDirection")); // Jump Action
            _thirdPersonCamera.Priority = 10;
        }

        public void Exit()
        {
            _thirdPersonCamera.Priority = 0;
        }

        public void Tick()
        {
            // Switch State if character is not jumping and is grounded
            if (_playerBlackboard.GetValue<bool>("IsGrounded"))
                _stateMachine.SetState(_explorationState);
        }

        public void FixedTick()
        {
            // Control movements in the air
            if (!_playerBlackboard.GetValue<bool>("IsGrounded")) 
                AirControl(_playerBlackboard.GetValue<Vector2>("MoveDirection"));
        }

        public void LateTick()
        {

        }

        #endregion

        #region Utils

        /// <summary>
        /// Apply upward force to initiate the jump.
        /// </summary>
        private void Jump(Vector2 moveDirection)
        {
            CameraDirection(moveDirection);

            // Apply upward force for jump and use the jumpDirection for horizontal movement
            _playerRigidbody.velocity = _jumpDirection * _playerStats.m_moveSpeed + new Vector3(0f, _playerStats.m_jumpForce, 0f);
        }

        /// <summary>
        /// Apply controlled movement in the air.
        /// </summary>
        /// <param name="moveDirection"></param>
        private void AirControl(Vector2 moveDirection)
        {
            CameraDirection(moveDirection);

            // Get the current velocity and apply horizontal air control
            Vector3 currentVelocity = _playerRigidbody.velocity;
            Vector3 targetVelocity = new Vector3(_jumpDirection.x * _playerStats.m_airControlSpeed, currentVelocity.y, _jumpDirection.z * _playerStats.m_airControlSpeed);

            // Smoothly transition to the target velocity in the air
            _playerRigidbody.velocity = Vector3.Lerp(
                currentVelocity,
                targetVelocity,
                _playerStats.m_airControlRotate * Time.fixedDeltaTime
            );
        }

        private void CameraDirection(Vector2 moveDirection)
        {
            // Get the camera's forward direction for movement
            _cameraForward = _mainCamera.transform.forward;
            _cameraRight = _mainCamera.transform.right;

            // Flatten the camera's forward and right vectors to the XZ plane
            _cameraForward.y = 0f;
            _cameraRight.y = 0f;
            _cameraForward.Normalize();
            _cameraRight.Normalize();

            // Convert 2D movement direction to 3D world space using camera orientation
            Vector3 jumpDirection = (_cameraForward * moveDirection.y + _cameraRight * moveDirection.x).normalized;

            _jumpDirection = jumpDirection;
        }

        #endregion

        #region Privates

        // Physics
        private Vector3 _jumpDirection;

        // Cameras
        private Vector3 _cameraForward;
        private Vector3 _cameraRight;
        private Transform _cameraTransform;

        #endregion
    }
}
