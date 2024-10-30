using UnityEngine;

namespace Data.Runtime
{
    public interface ICanTeleport
    {
        public void Teleport(Vector3 teleportPoint);
    }
}