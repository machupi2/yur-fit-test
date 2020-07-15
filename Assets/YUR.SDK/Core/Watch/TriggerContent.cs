using UnityEngine;
using UnityEngine.Events;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Turns on/off content on sleeves
    /// </summary>
    public class TriggerContent : MonoBehaviour
    {
        public GameObject[] contentObjects = null;
        public bool isActive = false;
        [SerializeField] private UnityEvent _onToggleOn = new UnityEvent(), _onToggleOff = new UnityEvent();
        public void Toggle(bool activeState)
        {
            isActive = activeState;

            if (activeState == true)
            {
                _onToggleOn.Invoke();
            }  else
            {
                _onToggleOff.Invoke();
            }

            for (int i = 0; i < contentObjects.Length; i++)
                contentObjects[i].SetActive(activeState);
        }
    } 
}