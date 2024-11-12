using DG.Tweening;
using UnityEngine;

namespace Objects.Runtime
{
    public class LockBillboard : MonoBehaviour
    {
        #region Unity API

        private void LateUpdate()
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }

        #endregion
    }
}