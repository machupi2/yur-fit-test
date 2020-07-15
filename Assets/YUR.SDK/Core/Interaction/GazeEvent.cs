using UnityEngine;
using UnityEngine.Events;

namespace YUR.SDK.Core.Interaction
{
    /// <summary>
    /// Event that handles interactions on the watch
    /// </summary>
    public class GazeEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _fireGaze = null;
        [SerializeField] private bool _fireEventManually = false;

        private void LateUpdate()
        {
            if (_fireEventManually)
            {
                _fireEventManually = false;
                FireGazeEvent();
            }
        }

        public void FireGazeEvent()
        {
            _fireGaze.Invoke();
        }
    }
}