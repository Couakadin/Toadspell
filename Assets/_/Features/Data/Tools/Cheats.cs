using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Runtime
{
    public class Cheats : MonoBehaviour
    {
        #region Unity API

        private void Start () 
        {
            _ctrl = KeyCode.LeftControl;
            _shift = KeyCode.LeftShift;
        }
    	void Update()
    	{
            if (Input.GetKey(_ctrl) && Input.GetKey(_shift) && Input.GetKeyDown(KeyCode.Alpha1)) Debug.Log("Cheat Code");
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private Blackboard _playerBlackboard;
        private KeyCode _ctrl;
        private KeyCode _shift;

        #endregion
    }
}