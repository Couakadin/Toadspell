using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = nameof(FloatEvent), menuName = Consts.SCRIPTABLES_SUBMENU_EVENTS + nameof(FloatEvent))]
    public class FloatEvent : GenericScriptableEvent<float> { }
}