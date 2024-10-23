using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = nameof(GameObjectEvent), menuName = Consts.SCRIPTABLES_SUBMENU_EVENTS + nameof(GameObjectEvent))]
    public class GameObjectEvent : GenericScriptableEvent<GameObject> { }
}