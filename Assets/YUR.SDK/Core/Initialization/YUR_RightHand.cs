using UnityEngine;
using YUR.SDK.Core.Watch;

namespace YUR.SDK.Core.Initialization
{
    /// <summary>
    /// Sets/Unsets the RightHand to the YURWatch component
    /// </summary>
    public class YUR_RightHand : YUR_Hand
    {
        internal override Transform yurAnchor { get => YURWatch.RightHandAnchor; set => YURWatch.RightHandAnchor = value; }
    } 
}