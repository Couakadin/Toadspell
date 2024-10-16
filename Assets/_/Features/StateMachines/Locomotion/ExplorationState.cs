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
            ApplyGravity(); // Appliquer la gravité à chaque frame

            // Si aucun mouvement n'est déclenché, appliquer la décélération pour les mouvements horizontaux uniquement
            if (moveDirection == Vector2.zero)
                ApplyDeceleration();
            else
            {
                // Direction avant par rapport à la caméra
                _cameraForward = _cameraTransform.forward;
                _cameraRight = _cameraTransform.right;

                // Aplatir l'axe Y pour s'assurer que le joueur se déplace uniquement au sol
                _cameraForward.y = 0;
                _cameraRight.y = 0;

                // Normaliser les vecteurs pour une vitesse de déplacement constante
                _cameraForward.Normalize();
                _cameraRight.Normalize();

                // Convertir le Vector2 moveDirection en une direction 3D relative à la caméra
                _cameraDirection = (_cameraForward * moveDirection.y + _cameraRight * moveDirection.x);

                // Rotation fluide du joueur vers la direction cible
                SmoothRotateTowards(_cameraDirection);

                // Appliquer le mouvement avec accélération
                ApplyMovement(_cameraDirection);
            }
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
        /// Applique une décélération uniquement aux mouvements horizontaux (X, Z).
        /// </summary>
        private void ApplyDeceleration()
        {
            // Obtenir la vélocité actuelle du joueur
            Vector3 currentVelocity = _playerRigidbody.velocity;

            // Ne ralentir que les mouvements horizontaux (X et Z)
            Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
            Vector3 deceleratedVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, _playerStats.m_deceleration * Time.fixedDeltaTime);

            // Combiner la décélération horizontale avec la vélocité verticale (Y)
            _playerRigidbody.velocity = new Vector3(deceleratedVelocity.x, currentVelocity.y, deceleratedVelocity.z);
        }

        /// <summary>
        /// Applique la gravité indépendamment des mouvements horizontaux.
        /// </summary>
        private void ApplyGravity()
        {
            Vector3 currentVelocity = _playerRigidbody.velocity;

            // Appliquer la gravité uniquement sur l'axe Y
            if (!_playerBlackboard.GetValue<bool>("IsGrounded"))
                currentVelocity.y += Physics.gravity.y * _playerStats.m_fallGravityMultiplier * Time.fixedDeltaTime;

            // Mettre à jour la vélocité du joueur
            _playerRigidbody.velocity = new Vector3(currentVelocity.x, currentVelocity.y, currentVelocity.z);
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
