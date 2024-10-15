using Data.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Runtime
{
    public class TeleportBehaviour : MonoBehaviour, ICanTeleport
    {
        public void Teleport(Transform teleportPoint)
        {
            transform.position = teleportPoint.position;
        }
    }
}