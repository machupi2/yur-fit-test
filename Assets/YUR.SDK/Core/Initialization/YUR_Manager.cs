//======= Copyright (c) YUR Inc., All rights reserved. ===============
// YUR_Manager implementation overview:
// The YUR_Manager is split into two major parts:
//     * OnYURUpdate: The inspector-accessible event that fires an OverlayStatusUpdate every second.
//     * Login: This public class has events that should be subscribed to before firing.

using UnityEngine;
using YUR.Fit.Core.Models;
using Debug = UnityEngine.Debug; ///Don't change this please. There is a YUR variant
using UnityEngine.Events;
using YUR.Fit.Unity;
using System.Runtime.CompilerServices;
using System.IO;
using YUR.Fit.Unity.Utilities;
using YUR.SDK.Core.Utils;
using YUR.SDK.Core.Configuration;
using static YUR.Fit.Unity.CoreServiceManager;
using System;
using YUR.Fit.Core.Watch;

namespace YUR.SDK.Core.Initialization
{
    /// <summary>
    /// Start a YUR Service from Unity and track workout data
    /// </summary>
    public partial class YUR_Manager : MonoBehaviour
    {
        /// <summary>
        /// Latest Update of Workout data
        /// </summary>
        public OverlayStatusUpdate LatestOSU { get; private set; }
        public WidgetApp[] LatestWidgetApps { get; private set; }
        public UserPreferences LatestPreferences { get; private set; }

        /// <summary>
        /// Singleton Instance for easy static functions calls
        /// </summary>
        private static YUR_Manager _instance = null;
        public static YUR_Manager Instance { get { return _instance; } }

        /// <summary>
        /// Events for circumstances
        /// </summary>
        [Header("YUR Runtime Events")]
        public UnityEvent OnYURStartup = null;                                      ///Fires the first time YUR updates
        public YUREvent OnYURUpdate = null;                                         ///Fires every time YUR updates

        public UserPreferencesEvent OnPreferenceUpdate = null;                      ///Fires when the user preferences are updated

        internal ShortcodeLoginEvent OnLoginAttempt = new ShortcodeLoginEvent();    ///Fires when a login is attempted. Returns a shortcode message
        internal UnityEvent OnLoginSuccess = new UnityEvent();                      ///Fires when a login is successful
        internal UnityEvent OnLoginFailed = new UnityEvent();                       ///Fires when a login fails

        public YUR_Settings YURSettings;
        
        [Header("DEBUG OPTIONS")]
        [Tooltip("Resource Heavy. Turn this off before making builds.")]
        public bool TurnOnDebugLogging = false;

        #region MonoBehaviour Callbacks

        // Create instance of YUR_Manager and begin the service
        private void OnEnable()
        {
            CoreServiceManager.WidgetAppsChangedAction += OnUpdateWidgetApps;
            bool initCore = false;

            try
            {
                if (_instance == null)
                {
                    _instance = this;
                    DontDestroyOnLoad(gameObject);

                    initCore = true;
                }
                else if (_instance != this)
                {
                    Destroy(gameObject);
                }
                else
                {
                    if (!CoreServiceManager.Initialized)
                    {
                        initCore = true;
                    }
                }
            } catch (UnityException e)
            {
                Instance.Log("Could not create instance. Here's why: " + e);
            }

            if (initCore)
            {
                try
                {
                    LatestOSU = null;

                    // Magic line that makes the YUR service run
                    CoreServiceManager.RunServices
                    (
                        OnOverlayUpdate: OnStatusUpdate /* Fires action every second */,
                        OnServiceShutdown: OnServiceShutdown,
                        OnUserPreferencesChanged : OnUserPreferencesChanged
                    );

                    Instance.Log("YUR Services Started!");
                }
                catch (UnityException e)
                {
                    Instance.Log("YUR Could Not Start. Here's Why: " + e.Message);
                }
            }
        }

        // If the object is disabled, check to see if the developer also wants to turn off our service
        private void OnDisable()
        {
            CoreServiceManager.WidgetAppsChangedAction -= OnUpdateWidgetApps;

            if (YURSettings.TurnOffYURServiceOnDisable)
            {
                CoreServiceManager.StopServices();
            }
        }
        #endregion

        #region YURStatusUpdates
        // This is used to update objects that use the YUR workout metrics
        private void OnStatusUpdate(OverlayStatusUpdate obj)
        {
            try
            {
                if (LatestOSU == null)
                {
                    // Fires on the first instance of getting an OverlayStatusUpdate object
                    Instance.Log("Starting YUR Service...");
                    OnYURStartup.Invoke();

                    LatestOSU = obj;
                }
            } catch (UnityException e)
            {
                Instance.Log("Could not fire startup event. Here's why: " + e.Message);
            }

            try
            {
                Instance.Log
                (
                    "Latest Status Update: "
                    + "\n" + "Game Name: " + LatestOSU.GameName
                    + "\n" + "Today Squat Count: " + LatestOSU.TodaySquatCount
                    + "\n" + "User Rank: " + LatestOSU.Rank
                    + "\n" + "Today's Calories: " + LatestOSU.TodayCalories
                    + "\n" + "Time in Workout: " + LatestOSU.SecondsInWorkout
                );

                OnYURUpdate.Invoke(obj);
            } catch (UnityException e)
            {
                Instance.Log("Couldn't fire update event. Here's why: " + e.Message);
            }
        }
        private void OnUpdateWidgetApps(WidgetApp[] apps)
        {
            Instance.Log("Apps were updated!");

            Instance.Log("YUR Plugin Version is: " + typeof(CoreServiceManager).Assembly.GetName().Version.ToString());

            LatestWidgetApps = apps;
        }

        private void OnUserPreferencesChanged(UserPreferences obj)
        {
            try
            {
                Instance.Log("Preference Changed Requested!");
                LatestPreferences = obj;
                OnPreferenceUpdate.Invoke(LatestPreferences);
            } catch (UnityException e)
            {
                Instance.Log("Couldn't fire preference change event. Here's why: " + e.Message);
            }
        }

        // Used to notify the developer that the service is shutting down
        private void OnServiceShutdown(ExecutionResult obj)
        {
            Instance.Log("Shutting Down...");
        } 
        #endregion

        #region PINLogin
        /// <summary>
        /// Sends a request to get a PIN to authenticate logins
        /// </summary>
        public void Login()
        {
            try
            {
                CoreServiceManager.ShortcodeLogin(OnLoginRequest, OnLoginResponseSuccess, OnLoginResponseFailure);
            }
            catch (UnityException e)
            {
                Instance.Log("YUR Could Not Log In. Here's Why: " + e);
            }
        }

        /// <summary>
        /// Logs out of the current user
        /// </summary>
        public void Logout()
        {
            try
            {
                CoreServiceManager.Logout();
            } catch (UnityException e)
            {
                Instance.Log("YUR could not log out. Here's why: " + e.Message);
            }
        }

        // Add listeners to the OnLoginAttempt event in order to get a 
        // PIN to log in with on https://app.yur.fit/code or yur.watch
        private void OnLoginRequest(YURShortcodeResponse obj)
        {
            try
            {
                Instance.Log("Attempting to Log In...");
                OnLoginAttempt.Invoke(obj);
            } catch (UnityException e)
            {
                Instance.Log("Could not get login request. Here's why: " + e.Message);
            }
        }

        // Fired if able to log in
        private void OnLoginResponseSuccess()
        {
            try
            {
                Instance.Log("YUR Logged In!");
                OnLoginSuccess.Invoke();
            }
            catch (UnityException e)
            {
                Instance.Log("Could not fire login success. Here's why: " + e.Message);
            }
        }

        // Fired if user cannot retrieve PIN or otherwise could not log in
        private void OnLoginResponseFailure(string obj)
        {
            try
            {
                Instance.Log("YUR Login Failed!");
                OnLoginFailed.Invoke();
            }
            catch (UnityException e)
            {
                Instance.Log("Could not fire login failure. Here's why: " + e.Message);
            }
        } 
        #endregion

        /// <summary>
        /// Log that records script that fired message, the line it was fired on, and the message itself
        /// PLEASE NOTE THAT USING THIS IS RESOURCE INTENSIVE
        /// </summary>
        public void Log(object text,
        [CallerFilePath] string file = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
        {
            if (TurnOnDebugLogging)
                Debug.Log($"{Path.GetFileName(file)}->{member}({line}): {text.ToString()}");
        }
    }
}