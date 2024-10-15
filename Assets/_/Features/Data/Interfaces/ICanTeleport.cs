using UnityEngine;

namespace Data.Runtime
{
    public interface ICanTeleport
    {
        public void Teleport(Transform teleportPoint);
    }
}