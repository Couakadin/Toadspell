using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class CameraManager : MonoBehaviour
    {
        #region Publics

        #endregion


        #region Unity API
        private void Awake()
        {
            _frontTempleCamera.Priority = 11;
        }

        #endregion


        #region Main Methods
        
        public void ATempleFrontCameraPriorityZero()
        {
            _frontTempleCamera.Priority = 0;
        }

        public void ABridgeBossCameraFirstPriority()
        {
            _bridgeBossCamera.Priority = 12;
        }

        public void ABridgeCamerasReset()
        {
            _bridgeBossCamera.Priority = 0;
        }

        #endregion


        #region Utils

        #endregion


        #region Privates & Protected

        [SerializeField] private CinemachineVirtualCamera _frontTempleCamera;
        [SerializeField] private CinemachineVirtualCamera _bridgeBossCamera;

        #endregion
    }
}