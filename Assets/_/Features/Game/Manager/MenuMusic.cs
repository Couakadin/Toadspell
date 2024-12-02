using UnityEngine;

namespace Game.Runtime
{
    public class MenuMusic : MonoBehaviour
    {
        public static MenuMusic m_instance;
        private void Awake()
        {
            //if (m_instance != null && m_instance != this)
            //{
            //    Destroy(this);
            //}
            //else
            //{
            //    m_instance = this;
            //}

            DontDestroyOnLoad(transform.gameObject);
        }
        // Start is called before the first frame update
    }
}
