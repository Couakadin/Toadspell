using UnityEngine;

namespace Data.Runtime
{
    public interface IHurt
    {
        public void AddImpact(Vector3 dir, float force);
    }
}
