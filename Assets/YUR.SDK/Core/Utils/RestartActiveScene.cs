using UnityEngine;
using UnityEngine.SceneManagement;

namespace YUR.SDK.Core.Utils
{
    /// <summary>
    /// Used for testing the persistency of the YUR.SDK system
    /// </summary>
    public class RestartActiveScene : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}