using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchDemoScene : MonoBehaviour
{
    #region Unity API

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Application.Quit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
		SceneManager.LoadScene(1);
    }
	
    #endregion
}