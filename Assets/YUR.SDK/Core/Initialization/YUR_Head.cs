using UnityEngine;
using YUR.SDK.Core.Watch;

namespace YUR.SDK.Core.Initialization
{
    /// <summary>
    /// Sets/Unsets the Head to the YURWatch component
    /// </summary>
    public class YUR_Head : MonoBehaviour
    {
        private void OnEnable()
        {
            YURWatch.Head = gameObject;
        }

        private void OnDisable()
        {
            if (YURWatch.Head == gameObject)
            {
                YURWatch.Head = null;
            }
        }
    } 
}