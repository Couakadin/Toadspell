using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Runtime
{
    public class Cheats : MonoBehaviour
    {
        #region Unity API

    	void Update()
    	{
            if (Input.GetKey(KeyCode.LeftControl))
            {

            }
    	}

        #endregion


        #region Privates & Protected

        [SerializeField] private Blackboard _playerBlackboard;

        #endregion
    }
}