using System.Collections.Generic;
using UnityEngine;

namespace Data.Runtime
{
    public static class Consts
    {
        public const string SCRIPTABLES_SUBMENU = "Data/Scriptables/Lists/";
    }
    [CreateAssetMenu(fileName = "Transforms", menuName = "Data/Scriptables/Lists/Transforms")]
    public class TransformListData : ScriptableObject
    {
        #region Publics

	    public List<Transform> transformsList;

        #endregion
    }
}