using UnityEngine;
using YUR.SDK.Core.Configuration;
using YUR.SDK.Core.Initialization;
using YUR.SDK.Core.Enums;
using UnityEngine.Events;
using System;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Prefab used to create the YURWatch implementation of the YUR.SDK
    /// </summary>
    public class YURWatch : MonoBehaviour
    {
        internal UnityEvent OnHandChanged { get; set; }

        // Settings Field
        public YUR_Settings YURSettingsAsset = null;

        //Default Watch
        public GameObject DefaultWatch = null;

        // Hand Fields
        internal static GameObject Head = null;
        internal static Transform LeftHandAnchor = null;
        internal static Transform RightHandAnchor = null;

        // Dynamically Set References
        private GameObject m_watchContainer = null;
        private GameObject m_watch = null;
        private GameObject m_defaultWatch = null;
        private Vector3
            m_watchpos = Vector3.zero,
            m_watcheuler = Vector3.zero,
            m_watchscale = Vector3.one;

        private Action<GameObject, int> m_setLayer = null;

        // Instantiate initial watch pieces and set their layer
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            CreateWatch();
            SetDefaultSettings();
            SetLayers();
        }

        //Create structure for watch and loads watch prefab from Resources
        private void CreateWatch()
        {
            try
            {
                m_watchContainer = new GameObject("YUR.WatchContainer");
                m_watchContainer.transform.SetParent(transform);

                m_watch = Instantiate(Resources.Load("YURWatch\\YUR.Watch", typeof(GameObject)) as GameObject);
                m_watch.transform.SetParent(m_watchContainer.transform);
                m_defaultWatch = Instantiate(DefaultWatch, m_watch.transform);
                m_watchpos = m_watch.transform.position;
                m_watcheuler = m_watch.transform.eulerAngles;
                m_watchscale = m_watch.transform.localScale;

            } catch (UnityException e)
            {
                Debug.Log("Error Creating Watch: " + e.Message);
            }
        }

        // 
        private void SetDefaultSettings()
        {
            try
            {
                if (YURSettingsAsset.WatchAndTileShaderOverride != null)
                {
                    for (int i = 0; i < DefaultWatch.transform.childCount; i++)
                    {
                        DefaultWatch.transform.GetChild(i).GetComponent<MeshRenderer>().sharedMaterial.shader = YURSettingsAsset.WatchAndTileShaderOverride;
                    }
                }
            }
            catch (UnityException e)
            {
                Debug.Log(e.Message);
            }
        }

        // Recursively set layers of the watch
        internal void SetLayers()
        {
            try
            {
                m_setLayer = (go, layer) =>
                {
                    go.layer = layer;
                    for (int i = 0; i < go.transform.childCount; i++)
                    {
                        var t = go.transform.GetChild(i);
                        m_setLayer(t.gameObject, layer);
                    }
                };

                var setLayer = LayerMask.NameToLayer(YURSettingsAsset.LayerToSet);
                if (setLayer < 0)
                {
                    setLayer = 0;
                }

                m_setLayer(m_watch, setLayer);
            } catch(UnityException e)
            {
                Debug.Log("Could not set layers on the watch. Please make sure you have a proper layer set in your YURSettings: " + e.Message);
            }
        }

        // Follow the Hand Being Used (in the config asset)
        private void Update()
        {
            if (!(m_watch is null) && m_watch.activeSelf)
            {
                UpdateWatchTransform(YURSettingsAsset);
            }
        }

        // Updates the watch pos and rot
        private void UpdateWatchTransform(YUR_Settings settings)
        {
            try
            {
                if (!(LeftHandAnchor is null) && !(RightHandAnchor is null))
                {
                    switch (settings.HandBeingUsed)
                    {
                        case HandState.Left:
                            m_watchpos = LeftHandAnchor.position;
                            m_watcheuler = LeftHandAnchor.eulerAngles;
                            m_watch.transform.localPosition = settings.LeftPositionOffset;
                            m_watch.transform.localEulerAngles = settings.LeftEulerOffset;
                            m_watch.transform.localScale = new Vector3(-m_watchscale.x, m_watchscale.y, -m_watchscale.z);
                            break;
                        case HandState.Right:
                            m_watchpos = RightHandAnchor.position;
                            m_watcheuler = RightHandAnchor.eulerAngles;
                            m_watch.transform.localPosition = settings.RightPositionOffset;
                            m_watch.transform.localEulerAngles = settings.RightEulerOffset;
                            m_watch.transform.localScale = new Vector3(-m_watchscale.x, -m_watchscale.y, -m_watchscale.z);
                            break;
                        default:
                            m_watchpos = Vector3.zero;
                            m_watcheuler = Vector3.zero;
                            m_watch.transform.localScale = m_watchscale;
                            break;
                    }

                    m_watchContainer.transform.position = m_watchpos;
                    m_watchContainer.transform.eulerAngles = m_watcheuler;
                }
            }
            catch (MissingReferenceException e)
            {
                YUR_Manager.Instance.Log(e.Message);
                gameObject.SetActive(false);
            }
        }

        public void ToggleWatch(bool isActive)
        {
            if (m_defaultWatch != null)
            {
                m_defaultWatch.SetActive(isActive);
            } else
            {
                Debug.Log("There is default watch to toggle!");
            }
        }
    }
}
