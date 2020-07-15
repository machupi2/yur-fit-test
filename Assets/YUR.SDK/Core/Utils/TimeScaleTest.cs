using UnityEngine;

namespace YUR.SDK.Core.Utils
{
    /// <summary>
    /// Used for testing the functionality of the YUR.SDK system when timescale is set to 0
    /// </summary>
    public class TimeScaleTest : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Time.timeScale = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Time.timeScale = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Time.timeScale = 2;
            }
        }
    } 
}
