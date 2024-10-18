using Data.Runtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StateMachine.Runtime
{
    public class AimState : AState, IAmStateMachine
    {
        #region Methods

        public AimState(StateMachineCore stateMachine, Dictionary<string, object> parameterDictionary = null) : base(stateMachine, parameterDictionary)
        {
            this._stateMachine = stateMachine;
            this._parameterDictionary = parameterDictionary;

            this._aimImage = GetItemFromParameterDictionary<Image>("aimImage");
            _aimRectTransform = _aimImage.rectTransform;

            _mainCameraTransform = _shoulderCamera.transform;
        }

        public string Name() => "Aim";

        public void Enter()
        {
            _playerRigidbody.velocity = Vector3.zero;
            _shoulderCamera.Priority = 10;
            _aimImage.enabled = true;
            _shoulderCamera.ForceCameraPosition(_thirdPersonCamera.transform.position, _thirdPersonCamera.transform.rotation);
        }

        public void Exit()
        {
            _shoulderCamera.Priority = 0;
            _aimImage.enabled = false;
            _thirdPersonCamera.ForceCameraPosition(_shoulderCamera.transform.position, _shoulderCamera.transform.rotation);
        }

        public void Tick()
        {
            // Player following camera on axe X
            _cameraForward = _mainCameraTransform.forward;
            _cameraForward.y = 0;
            _targetRotation = Quaternion.LookRotation(_cameraForward);
            _playerTransform.rotation = _targetRotation;

            // WorldSpace position of aim cursor in front of the camera
            _rayAim = new Ray(_mainCameraTransform.position, _mainCameraTransform.forward);
            // Set the aim cursor position
            _aimRectTransform.position = _rayAim.GetPoint(_distanceFromCamera);
            _screenPosition = _mainCamera.WorldToScreenPoint(_rayAim.GetPoint(_distanceFromCamera));
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _aimRectTransform,
                _screenPosition,
                _mainCamera,
                out Vector2 localPosition
            );

            _aimRectTransform.localPosition = localPosition;

            if (!_playerBlackboard.GetValue<bool>("IsAiming"))
                _stateMachine.SetState(_explorationState);
        }

        public void FixedTick()
        {

        }

        public void LateTick()
        {

        }

        #endregion

        #region Privates

        private Image _aimImage;
        private RectTransform _aimRectTransform;
        private Vector3 _cameraForward;
        private Quaternion _targetRotation;
        private Ray _rayAim;
        private Vector3 _screenPosition;
        private float _distanceFromCamera = 1f;

        #endregion
    }
}
