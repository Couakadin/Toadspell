using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class MenuMusic : MonoBehaviour
    {

        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
        }
        // Start is called before the first frame update
    }
}
