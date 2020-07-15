using System;
using UnityEngine;
using YUR.Fit.Core.Models;
using YUR.SDK.Core.Initialization;
using YUR.SDK.Core.Utils;
using YUR.SDK.Core.Watch;

namespace YUR.SDK.Core.AppData
{
    /// <summary>
    /// Sets the Tile Color from the Server
    /// </summary>
    public class YUR_TileColor : DataSetter
    {
        private Renderer m_rend = null;
        private string m_arbDataPath = string.Empty;
        SetFillColor SFC = null;

        public override void Awake()
        {
            base.Awake();

            m_arbDataPath = $"{gameObject.name}.{gameObject.name}.{DataTag}";
        }

        protected override string DefaultDataTag => "tilecolor";

        public override void ApplyOverlayUpdate(OverlayStatusUpdate OSU)
        {
            base.ApplyOverlayUpdate(OSU);

            if (SFC == null)
            {
                SFC = transform.parent.transform.parent.GetChild(0).gameObject.GetComponent<SetFillColor>();
            }

            if (SFC != null && SFC.enabled)
            {
                SFC.enabled = false;
            }

            if (m_rend == null) 
            {
                m_rend = transform.parent.transform.parent.GetChild(0).gameObject.GetComponent<Renderer>();
            }

            string arbText = OSU.WidgetAppData?.TryGetPropertyPath<string>(m_arbDataPath);

            try
            {
                if (!string.IsNullOrEmpty(arbText))
                {
                    m_rend.material.color = YUR_Formatter.ConvertToColor(arbText);
                }
            } catch (Exception e)
            {
                YUR_Manager.Instance.Log($"Could not get update because {e.Message}");
                YUR_Manager.Instance.OnYURUpdate.RemoveListener(ApplyOverlayUpdate);
            }
        }
    } 
}
