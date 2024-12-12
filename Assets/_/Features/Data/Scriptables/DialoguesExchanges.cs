using System.Collections.Generic;
using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = nameof(DialoguesExchanges), menuName = Consts.SCRIPTABLES_LINES + nameof(DialoguesExchanges))]
    public class DialoguesExchanges : ScriptableObject
    {
        public List<Dialogue> m_Dialogues;
        public bool m_hasAlreadyBeenDisplayed = false;
    }
}