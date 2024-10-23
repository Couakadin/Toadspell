using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = nameof(MyEvent), menuName = Consts.SCRIPTABLES_SUBMENU + nameof(MyEvent))]
    public class Vector3Event : GenericScriptableEvent<Vector3> { }
}