using System.Collections.Generic;
using UnityEngine;

namespace Data.Runtime
{

    [CreateAssetMenu(fileName = "Transforms", menuName = "Data/Scriptables/Lists/Transforms")]
    public class TransformListData : ScriptableObject
    {
        #region Publics

	    public List<Transform> transformsList;

        #endregion
    }
}