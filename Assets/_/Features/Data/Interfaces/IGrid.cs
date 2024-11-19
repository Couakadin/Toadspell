using UnityEngine;

namespace Data.Runtime
{
    public interface IGrid
    {
        public GameObject m_centralPlatform { get; set; }

        public GameObject GetRandomPlatform();
    }
}
