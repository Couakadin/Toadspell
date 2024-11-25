using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.Runtime
{
    public class HealthBarBillboardBehaviour : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            transform.Rotate(0, 180, 0);
        }
    }
}