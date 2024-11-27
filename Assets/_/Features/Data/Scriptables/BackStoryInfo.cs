using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = nameof(Dialogue), menuName = Consts.SCRIPTABLES_LINES + nameof(BackStoryInfo))]
    public class BackStoryInfo : ScriptableObject
    {
        public Sprite m_background;
        public DialogueLine m_line;
    }
}
