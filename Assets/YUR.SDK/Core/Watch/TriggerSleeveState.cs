using UnityEngine;
using YUR.SDK.Core.Enums;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Tells the YURSleeveController when to change sleeves
    /// </summary>
    public class TriggerSleeveState : MonoBehaviour
    {
        public SleeveState StateToTransitionTo = SleeveState.StartSleeve;

        internal YURSleeveController referenceToSleeveController = null;

        public void SendTransitionStateToController()
        {
            referenceToSleeveController.CurrentSleeveState = StateToTransitionTo;
        }
    } 
}
