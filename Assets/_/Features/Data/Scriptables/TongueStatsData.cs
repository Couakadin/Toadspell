using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Runtime
{
    [CreateAssetMenu(fileName = "TongueStats", menuName = "Data/Scriptables")]
    public class TongueStatsData : ScriptableObject
    {
        #region Publics

        public float speed = 2f;
        public float coolDown = 3f;
        public float grabDelay = 1f;
        public float maxDistance = 10f;
        public float coolDownTimer = 1f;

        #endregion
    }
}