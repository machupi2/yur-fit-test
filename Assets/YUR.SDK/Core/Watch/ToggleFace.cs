using UnityEngine;
using YUR.SDK.Core.Initialization;

namespace YUR.SDK.Core.Watch
{
    public class ToggleFace : MonoBehaviour
    {
        public void Set(bool activeState)
        {
            try
            {
                transform.GetChild(0).gameObject.SetActive(activeState);
            } catch (UnityException e)
            {
                YUR_Manager.Instance.Log($"Could not load face because {e.Message}");
            }
            
        }
    } 
}
