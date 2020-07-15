using System;
using System.Linq;
using UnityEngine;
using YUR.Fit.Core.Models;
using YUR.SDK.Core.Initialization;
using YUR.SDK.Core.Utils;
using YUR.SDK.Core.Watch;

namespace YUR.SDK.Core.AppData
{
    /// <summary>
    /// Sets the Emission from the Server
    /// </summary>
    public class YUR_Emission : DataSetter
    {
        private Renderer m_rend = null;

        public override void Awake()
        {
            base.Awake();

            m_rend = GetComponent<Renderer>();
            m_rend.material.EnableKeyword("_EMISSION");
        }
        protected override string DefaultDataTag => "emission"; //lol, emission

        public override void ApplyOverlayUpdate(OverlayStatusUpdate OSU)
        {
            base.ApplyOverlayUpdate(OSU);

            string[] arbArray = OSU.WidgetAppData?.TryGetPropertyPath<string[]>(PropertyPath);

            if (arbArray != null)
            {
                string[] arr = arbArray.ToArray();

                try
                {
                    m_rend.material.SetColor("_EmissionColor", YUR_Formatter.ConvertToColor(arr[0]));
                    m_rend.material.SetFloat("_Emission", float.Parse(arr[1]));
                }
                catch (Exception e)
                {
                    YUR_Manager.Instance.Log($"Could not get update because {e.Message}");
                    YUR_Manager.Instance.OnYURUpdate.RemoveListener(ApplyOverlayUpdate);
                }
            }
        }

    } 
}
