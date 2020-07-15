using UnityEngine.Events;
using YUR.Fit.Core.Models;
using System;
using YUR.Fit.Unity.Utilities;
using YUR.Fit.Core.Watch;

namespace YUR.SDK.Core.Utils
{
    /// <summary>
    /// A collection of UnityEvent Extensions useful for sending events with specific parameters
    /// </summary>
    [Serializable]
    public class YUREvent : UnityEvent<OverlayStatusUpdate> { }

    [Serializable]
    public class UnityEventString : UnityEvent<string> { }

    [Serializable]
    public class UnityEventFloat : UnityEvent<float> { }

    [Serializable]
    public class UnityEventInt : UnityEvent<int> { }

    [Serializable]
    public class ShortcodeLoginEvent : UnityEvent<YURShortcodeResponse> { }

    [Serializable]
    public class UserPreferencesEvent : UnityEvent<UserPreferences> { }

    [Serializable]
    public class UnityEventTwoFloat : UnityEvent<float, float> { }

    [Serializable]
    public class WidgetAppsEvent : UnityEvent<WidgetApp[]> { }
}
