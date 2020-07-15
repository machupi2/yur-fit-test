using System.Collections;
using UnityEngine;
using YUR.SDK.Core.Watch;

namespace YUR.SDK.Core.Initialization
{
    /// <summary>
    /// Creates an anchor for use by the watch
    /// </summary>
    public class YUR_Hand : MonoBehaviour
    {
        internal virtual Transform yurAnchor { get; set; }
        internal GameObject thisAnchor = null;

        private void Awake()
        {
            thisAnchor = new GameObject("YUR_Hand");
            thisAnchor.transform.SetParent(transform, false);
        }

        private void OnEnable()
        {
            yurAnchor = thisAnchor.transform;
        }

        private void OnDisable()
        {
            if (yurAnchor == thisAnchor.transform)
            {
                yurAnchor = null;
            }
        }
    }
}
