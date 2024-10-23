using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = nameof(Vector3Event), menuName = Consts.SCRIPTABLES_SUBMENU_EVENTS + nameof(Vector3Event))]
    public class Vector3Event : GenericScriptableEvent<Vector3> { }
}