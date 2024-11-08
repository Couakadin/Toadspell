using UnityEngine;
using UnityEngine.UI;

namespace Data.Runtime
{
    [System.Serializable]
    public struct DialogueLine
    {
        public string m_sentence;
        public float m_screenTime;
        public Sprite m_image;
    }
}