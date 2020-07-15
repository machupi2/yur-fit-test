using YUR.Fit.Core.Models;

namespace YUR.SDK.Core.Interfaces
{
    /// <summary>
    /// Used to update any objects on the watch that rely on the OverlayStatusUpdate
    /// </summary>
    public interface IYURUpdatable
    {
        void ApplyOverlayUpdate(OverlayStatusUpdate OSU);
    }
}