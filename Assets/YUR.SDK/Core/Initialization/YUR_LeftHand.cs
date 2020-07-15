using UnityEngine;
using YUR.SDK.Core.Watch;

namespace YUR.SDK.Core.Initialization
{
    /// <summary>
    /// Sets/Unsets the LeftHand to the YURWatch component
    /// </summary>
    public class YUR_LeftHand : YUR_Hand
    {
        internal override Transform yurAnchor { get => YURWatch.LeftHandAnchor; set => YURWatch.LeftHandAnchor = value; }
    } 
}