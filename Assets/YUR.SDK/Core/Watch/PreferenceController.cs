using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YUR.Fit.Core.Models;
using YUR.SDK.Core.Initialization;

namespace YUR.SDK.Core.Watch
{
    public class PreferenceController : MonoBehaviour
    {
        public UnityEvent OnPreferenceUpdate = null;

        private void OnEnable()
        {
            YUR_Manager.Instance.OnPreferenceUpdate.AddListener(Reload);
        }

        private void OnDisable()
        {
            YUR_Manager.Instance.OnPreferenceUpdate.RemoveListener(Reload);
        }

        private void Reload(UserPreferences arg0)
        {
            OnPreferenceUpdate.Invoke();
        }
    } 
}
