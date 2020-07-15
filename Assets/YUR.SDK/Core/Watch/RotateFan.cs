using UnityEngine;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Rotates fan on the Legendary Watch
    /// </summary>
    public class RotateFan : MonoBehaviour
    {
        [SerializeField] private float _fanSpeed = 5.0f;

        private void Update()
        {
            transform.Rotate(new Vector3(0, 0, 1 * _fanSpeed), Space.Self);
        }
    } 
}
