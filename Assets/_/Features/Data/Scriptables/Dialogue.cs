using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = nameof(Dialogue), menuName = Consts.SCRIPTABLES_LINES + nameof(Dialogue))]
    public class Dialogue : ScriptableObject
    {
        #region Publics

        public Sprite m_image;
        public string m_speakerName;
        public List<DialogueLine> m_lines;

        #endregion
    }
}