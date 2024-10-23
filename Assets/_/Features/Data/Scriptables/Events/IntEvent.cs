using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = nameof(IntEvent), menuName = Consts.SCRIPTABLES_SUBMENU_EVENTS + nameof(IntEvent))]
    public class IntEvent : GenericScriptableEvent<int> { }
}