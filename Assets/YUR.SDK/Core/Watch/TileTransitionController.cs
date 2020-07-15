using UnityEngine;
using YUR.Fit.Unity;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Handles how to transition the Tile Sleeve
    /// </summary>
    public class TileTransitionController : MonoBehaviour
    {
        public TriggerSleeveState triggerSleeveState = null;

        public void Logout()
        {
            if (CoreServiceManager.IsLoggedIn)
            {
                CoreServiceManager.Logout();
            }
            else
            {
                if (triggerSleeveState is object)
                    triggerSleeveState.SendTransitionStateToController();
            }
        }
    } 
}
