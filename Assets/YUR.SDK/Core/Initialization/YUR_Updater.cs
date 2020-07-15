using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using YUR.Fit.Core.Models;
using YUR.Fit.Core.Watch;
using YUR.Fit.Unity;
using YUR.SDK.Core.Utils;
using YUR.SDK.Core.Watch;

/// <summary>
/// Updates assets of the YUR Watch
/// </summary>
namespace YUR.SDK.Core.Initialization
{
    public class YUR_Updater : MonoBehaviour
    {
        public string LocalPath { get; private set; }

        public UnityEvent OnUpdateCheck = null;
        public UnityEvent OnUpdateRequired = null;
        public UnityEventTwoFloat OnDownloadProgressUpdate = null;
        public UnityEvent OnSaveComplete = null;

        private string m_version = string.Empty;
        private bool m_updated = false;

        private void Awake()
        {
            m_version = Application.unityVersion;

            LocalPath = Path.Combine
            (
                Application.persistentDataPath, // We store everything here for easy management
                ".yurfit", // That's us
                "assetbundles"
            );
        }

        private void LateUpdate()
        {
            if (YUR_Manager.Instance.LatestWidgetApps != null && !m_updated)
            {
                YUR_Manager.Instance.Log("Checking for updates...");
                m_updated = true;
                CheckForUpdates(YUR_Manager.Instance.LatestWidgetApps);
            }
        }

        private void OnEnable()
        {
            if (m_updated)
            {
                YUR_Manager.Instance.Log("Moving on");
                OnSaveComplete.Invoke();
            }
        }

        /// Checks if we need to update
        private void CheckForUpdates (WidgetApp[] apps)
        {
            OnUpdateCheck.Invoke();

            Dictionary<string, string> appsToUpdate = new Dictionary<string, string>();

            //For ad hoc testing
            //UpdateYURWatch();

            // Check for updates daily
            string lastUpdate = PlayerPrefs.GetString("YUR_LastUpdate");

            if (string.IsNullOrEmpty(lastUpdate) || !Directory.Exists(LocalPath))
            {
                YUR_Manager.Instance.Log("Running first update...");
                
                if (CheckForDirectory())
                {
                    try
                    {
                        Dictionary<string, string> appsToDownload = GetAppsToUpdate(apps);

                        if (appsToDownload.Count > 0)
                        {
                            StartCoroutine(StartDownloads(appsToDownload));
                        }
                    }
                    catch (UnityException e)
                    {
                        YUR_Manager.Instance.Log($"Had a problem getting apps to update. Reason: {e.Message}");
                    }
                }

                OnUpdateRequired.Invoke();
            }
            else
            {
                if ((DateTime.Parse(lastUpdate) - DateTime.Now.Date).Days >= 1)
                {
                    YUR_Manager.Instance.Log("Auto Updating...");

                    if (CheckForDirectory())
                    {
                        try
                        {
                            Dictionary<string, string> appsToDownload = GetAppsToUpdate(apps);

                            if (appsToDownload.Count > 0)
                            {
                                StartCoroutine(StartDownloads(appsToDownload));
                            }
                        } catch (UnityException e)
                        {
                            YUR_Manager.Instance.Log($"Had a problem getting apps to update. Reason: {e.Message}");
                        }
                    }

                    OnUpdateRequired.Invoke();
                }
                else
                {
                    YUR_Manager.Instance.Log("Update not required...");
                    OnSaveComplete.Invoke();
                }
            }
        }

        /// <summary>
        /// If a directory doesn't exist, create it.
        /// </summary>
        public bool CheckForDirectory()
        {
            try
            {
                if (!Directory.Exists(LocalPath))
                {
                    Directory.CreateDirectory(LocalPath);
                }
                return true;
            }
            catch (Exception e)
            {
                YUR_Manager.Instance.Log("Had a problem creating : " + e.Message);
                return false;
            }
        }

        private Dictionary<string, string> GetAppsToUpdate(WidgetApp[] apps)
        {
            Dictionary<string, string> appsToUpdate = new Dictionary<string, string>();

            foreach (var app in apps)
            {
                string widgetName;

#if UNITY_ANDROID
                var flatApp = app.ToPlatform
                (
                    Fit.Core.Watch.WidgetAppPlatform.UnitySDK,
                    Fit.Core.Watch.WidgetAppResourceType.AndroidAsset,
                    typeof(CoreServiceManager).Assembly.GetName().Version.ToString(),
                    m_version
                );
#endif

#if UNITY_STANDALONE_WIN
                var flatApp = app.ToPlatform
                (
                    Fit.Core.Watch.WidgetAppPlatform.UnitySDK,
                    Fit.Core.Watch.WidgetAppResourceType.WindowsAsset,
                    typeof(CoreServiceManager).Assembly.GetName().Version.ToString(),
                    m_version
                );
#endif

                if (flatApp.HasResource(WidgetAppFeatureType.Widget))
                {
                    YUR_Manager.Instance.Log
                    (
                        $"Found widget unity resource for app {flatApp.App.WidgetAppID} with a version of {flatApp.App.AppVersion} at URL {flatApp.GetResource(WidgetAppFeatureType.Widget).Resource.DownloadUri}"
                    );

                    widgetName = (flatApp.App.WidgetAppID + "-" + flatApp.App.AppVersion).ToLower().Replace('.','_');

                    if (!File.Exists(Path.Combine(LocalPath + widgetName)))
                    {
                        appsToUpdate.Add(widgetName, flatApp.GetResource(WidgetAppFeatureType.Widget).Resource.DownloadUri);
                    }
                }
            }

            return appsToUpdate;
        }


        private IEnumerator StartDownloads(Dictionary<string, string> apps)
        {
            Dictionary<string, byte[]> bundlesToSave = new Dictionary<string, byte[]>();

            int assetsDownloaded = 0;

            // Get Bundles
            foreach (var app in apps)
            {
                using (UnityWebRequest uwr = UnityWebRequest.Get(app.Value))
                {
                    yield return uwr.SendWebRequest();

                    if (uwr.isNetworkError || uwr.isHttpError)
                    {
                        YUR_Manager.Instance.Log(uwr.error);
                    }
                    else
                    {
                        // Get downloaded manifest
                        bundlesToSave.Add(app.Key, uwr.downloadHandler.data);
                        assetsDownloaded++;
                        OnDownloadProgressUpdate.Invoke(assetsDownloaded, apps.Count);
                    }
                }

                YUR_Manager.Instance.Log(assetsDownloaded + "/" + apps.Count);
            }

            // Save the bundles
            SaveBundles(bundlesToSave);
        }

        private void SaveBundles(Dictionary<string, byte[]> bundleDataAssets)
        {
            Dictionary<string, string> allCurrentBundleNamesAndVersion = new Dictionary<string, string>();

            foreach(var currentAsset in Directory.GetFiles(LocalPath))
            {
                string[] assetNameParts = currentAsset.Split('-');
                allCurrentBundleNamesAndVersion.Add(assetNameParts[0], assetNameParts[1]);
            }

            foreach (var asset in bundleDataAssets)
            {
                string path = Path.Combine(new string[] { LocalPath, asset.Key.ToLower() });

                // Delete the old file
                foreach (var nameAndVersion in allCurrentBundleNamesAndVersion)
                {
                    string[] assetToSave = asset.Key.Split('-');

                    // See if any of the already saved files match the one we want to save
                    if (nameAndVersion.Key == assetToSave[0])
                    {
                        Directory.Delete(Path.Combine(LocalPath, (nameAndVersion.Key + "-" + nameAndVersion.Value)));
                    }
                }

                // Create the File path if it does not exist
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }

                try
                {
                    File.WriteAllBytes(path, asset.Value);
                    YUR_Manager.Instance.Log("Saved Data to: " + path.Replace("/", "\\"));
                }
                catch (Exception e)
                {
                    YUR_Manager.Instance.Log("Failed To Save Data to: " + path.Replace("/", "\\"));
                    YUR_Manager.Instance.Log("Error: " + e.Message);
                }
            }

            print("Bundle Save Complete!");
            PlayerPrefs.SetString("YUR_LastUpdate", DateTime.Now.ToString());
            OnSaveComplete.Invoke();
        }
    } 
}
