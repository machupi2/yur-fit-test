using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using YUR.Fit.Core.Models;
using YUR.Fit.Core.Watch;
using YUR.Fit.Unity;
using YUR.SDK.Core.Watch;

namespace YUR.SDK.Core.Initialization
{
    [Serializable]
    class Watch
    {
        public string WatchName;
        public int MinLevelRequired;

        public Watch()
        {
            WatchName = "commonwatch";
            MinLevelRequired = 0;
        }
    }

    public class YUR_Loader : MonoBehaviour
    {
        public string LocalPath { get => _localPath; private set => _localPath = value; }
        private string _localPath = null;

        public bool LatestFilesUpdated { get => _latestFilesUpdated; set => _latestFilesUpdated = value; }
        [SerializeField] private bool _latestFilesUpdated = false;

        [SerializeField] private SetWidgetPlacements _widgetParent = null;
        [SerializeField] private Transform _watchAnchor = null;
        [SerializeField] private Watch[] _watches = null;
        [SerializeField] private UnityEvent _onLoadSuccess = null;
        [SerializeField] private UnityEvent _onLoadFailure = null;

        private WatchReferenceContainer m_instantiatedWatchReferences = null;
        private List<GameObject> m_widgets = new List<GameObject>();
        private List<AssetBundle> m_bundlesInUse = new List<AssetBundle>();
        private List<GameObject> m_defaultObjects = new List<GameObject>();

        private void Awake()
        {
            LocalPath = Path.Combine
            (
                Application.persistentDataPath, // We store everything here for easy management
                ".yurfit", // That's us
                "assetbundles"
            );
        }

        private void LateUpdate()
        {
            if (YUR_Manager.Instance.LatestOSU != null && LatestFilesUpdated && YUR_Manager.Instance.LatestPreferences != null)
            {
                LatestFilesUpdated = false;

                YUR_Manager.Instance.Log("Loading Assets...");

                Load(YUR_Manager.Instance.LatestOSU, YUR_Manager.Instance.LatestPreferences);
            }
        }

        public void UpdateAssets()
        {
            Load(YUR_Manager.Instance.LatestOSU, YUR_Manager.Instance.LatestPreferences);
        }

        #region Watch Loading
        /// <summary>
        /// Updates Watch to use based on User Rank
        /// </summary>
        /// <param name="OSU"></param>
        public void Load(OverlayStatusUpdate OSU, UserPreferences prefs)
        {
            int highestWatchLevel = 0;

            for (int i = 0; i < _watches.Length; i++)
            {
                if (OSU.Rank >= _watches[i].MinLevelRequired)
                {
                    highestWatchLevel = i;
                }
            }

            LoadWatch(highestWatchLevel, prefs);
        }

        private void LoadWatch(int watchIndex, UserPreferences prefs)
        {
            if (m_instantiatedWatchReferences is object)
            {
                Destroy(m_instantiatedWatchReferences.ThisWatch);
                m_instantiatedWatchReferences = null;
            }

            try
            {
                GameObject watch = Resources.Load(_watches[watchIndex].WatchName) as GameObject;

                watch = Instantiate(watch as GameObject, _watchAnchor);

                m_instantiatedWatchReferences = watch.GetComponent<WatchReferenceContainer>();

                if (YUR_Manager.Instance.YURSettings.WatchAndTileShaderOverride != null)
                {
                    m_instantiatedWatchReferences.SetMaterial(YUR_Manager.Instance.YURSettings.WatchAndTileShaderOverride);
                }

                m_instantiatedWatchReferences.SetRenderQueue(YUR_Manager.Instance.YURSettings.WatchRenderQueue);

                YUR_Manager.Instance.Log("Watch Bundle Loaded");

                YUR_Manager.Instance.gameObject.transform.parent.transform.parent.GetComponent<YURWatch>().ToggleWatch(false);

            } catch (UnityException e)
            {
                YUR_Manager.Instance.Log($"Could not load the requested watch. Falling back to default. Here's more info: {e.Message}");
            }

            UpdateWidgets(prefs);
        }
        #endregion Watch Loading

        #region Widget Loading
        /// <summary>
        /// Updates Tiles to use based on Preferences
        /// </summary>
        public void UpdateWidgets(UserPreferences prefs)
        {
            try
            {
                //Loop through Widget Array and load widget prefabs from Resources
                if (Directory.Exists(LocalPath))
                {
                    if (m_widgets.Count > 0)
                    {
                        foreach (GameObject widget in m_widgets)
                        {
                            Destroy(widget);
                        }
                    }

                    m_widgets.Clear();
                    _widgetParent.ClearWidgets();

                    foreach (AssetBundle bundle in m_bundlesInUse)
                    {
                        if (bundle != null)
                            bundle.Unload(false);
                    }

                    m_bundlesInUse.Clear();

                    StartCoroutine(LoadWidgets(prefs));
                }
                else
                {
                    YUR_Manager.Instance.Log("YUR Folder not Found!");
                }
            }
            catch (UnityException e)
            {
                YUR_Manager.Instance.Log("YUR Service could not load widgets. Here's why: " + e);
            }
        }

        private IEnumerator LoadWidgets(UserPreferences prefs)
        {
            try
            {
                string[] fileNames = Directory.GetFiles(Path.Combine(LocalPath));

                Dictionary<string,string> delimitedFiles = new Dictionary<string, string>();

                foreach (var fileName in fileNames)
                {
                    var specificName = Path.GetFileName(fileName);

                    // Key is the file name without a version
                    // Value is the actual name of the file for pulls
                    delimitedFiles.Add(specificName.Split('-')[0].ToLower(), specificName);
                }

                Dictionary<WidgetPosition, WidgetConfig> prefsToUse = prefs?.WatchConfig?.Widgets ?? UserPreferences.Stub.WatchConfig.Widgets;

                foreach (KeyValuePair<WidgetPosition, WidgetConfig> kvp in prefsToUse)
                {
                    string fileNameToGet = string.Empty;

                    foreach (var delimitedFile in delimitedFiles)
                    {
                        if (delimitedFile.Key == kvp.Value.WidgetTypeID.ToLower())
                        {
                            fileNameToGet = delimitedFile.Value;
                        }
                    }

                    string path = Path.Combine(LocalPath, fileNameToGet);
                    AssetBundle bundle = null;

                    if (File.Exists(path))
                    {
                        try
                        {
                            bundle = AssetBundle.LoadFromFile(path);
                        }
                        catch (UnityException e)
                        {
                            YUR_Manager.Instance.Log("Tried loading an assetbundle, but failed because " + e.Message);
                        }

                        if (bundle == null)
                        {
                            YUR_Manager.Instance.Log("Failed to load AssetBundle!");
                            yield break;
                        }

                        SpawnWidget(bundle.LoadAllAssets(), kvp.Key);

                        bundle.Unload(false);
                        m_bundlesInUse.Add(bundle);
                    }
                    else
                    {
                        SpawnDefaultWidget(kvp.Key);
                        YUR_Manager.Instance.Log("Could not find a file for bundle " + kvp.Key + " at path " + path);
                    }
                }

                YUR_Manager.Instance.gameObject.transform.parent.transform.parent.GetComponent<YURWatch>().SetLayers();

                _onLoadSuccess.Invoke();
            } catch (NullReferenceException e)
            {
                YUR_Manager.Instance.Log($"User Preferences could not be found because {e.Message}. Shutting down...");
                _onLoadFailure.Invoke();
            } catch (UnityException e)
            {
                YUR_Manager.Instance.Log($"User Preferences could not be found because {e.Message}. Shutting down...");
                _onLoadFailure.Invoke();
            }
        }

        private void SpawnDefaultWidget(WidgetPosition key)
        {
            UnityEngine.Object defaultResource = Resources.Load($"Tiles/{key}");

            GameObject go = Instantiate(defaultResource) as GameObject;

            ConfigureWidget(go, key);
        }

        /// Instantiates the widget prefab and places it in the correct location
        private void SpawnWidget(UnityEngine.Object[] assets, WidgetPosition widgetEnum)
        {
            GameObject thisWidget = null;

            // Parse array of known assets
            foreach (UnityEngine.Object asset in assets)
            {
                if (asset.GetType() == typeof(GameObject))
                {
                    thisWidget = Instantiate(asset) as GameObject;
                    ConfigureWidget(thisWidget, widgetEnum);
                }
            }
        }

        private void ConfigureWidget(GameObject thisWidget, WidgetPosition widgetEnum)
        {
            //Check if Widget Exists
            if (thisWidget is object)
            {
                //Get parent to set widget to
                Transform parentTile = _widgetParent.GetSlot(widgetEnum);

                //Making Sure Widget is properly anchored to sleeve
                thisWidget.transform.SetParent(parentTile, false);

                //Set widget transform
                thisWidget.transform.localPosition = UnityEngine.Vector3.zero;
                thisWidget.transform.localRotation = Quaternion.identity;
                thisWidget.transform.localScale = new UnityEngine.Vector3(0.5f, 0.5f, 0.5f);

                //Get the tile mesh as an object
                GameObject tile = parentTile.parent.GetChild(0).gameObject;

                if (tile is object)
                {
                    MeshRenderer tileRenderer = tile.GetComponent<MeshRenderer>();

                    if (tileRenderer != null)
                    {
                        SetTileColor STC = thisWidget.GetComponent<SetTileColor>();
                        SetFillColor SFC = tile.GetComponent<SetFillColor>();

                        if (STC is object)
                        {
                            if (SFC is object)
                                SFC.FillColor = thisWidget.GetComponent<SetTileColor>().TileColorOverride;
                        }
                        else
                        {
                            YUR_Manager.Instance.Log("No Tile Color Override Found on Widget");

                            if (SFC is object)
                            {
                                SFC.FillColor = Enums.YurFillColors.Default;
                            }
                        }

                        if (YUR_Manager.Instance.YURSettings.WatchAndTileShaderOverride != null)
                        {
                            tileRenderer.material.shader = YUR_Manager.Instance.YURSettings.WatchAndTileShaderOverride;
                            if (!tileRenderer.material.HasProperty(YUR_Manager.Instance.YURSettings.ShaderColorProperty))
                            {
                                YUR_Manager.Instance.Log("Material does not have Color Property to set! Reverting...");
                                tileRenderer.material.shader = Shader.Find("Unlit/Color");
                            }
                        }

                        tileRenderer.material.renderQueue = YUR_Manager.Instance.YURSettings.WatchRenderQueue;

                    }
                }

                //Add widget to list to update later
                m_widgets.Add(thisWidget);
            }
            else
            {
                YUR_Manager.Instance.Log("Widget Does not Exist!");
            }
        }
        #endregion Widget Loading
    }
}
