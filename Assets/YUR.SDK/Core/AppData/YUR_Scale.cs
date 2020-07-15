using System;
using UnityEngine;
using YUR.Fit.Core.Models;
using YUR.SDK.Core.Initialization;
using YUR.SDK.Core.Utils;
using YUR.SDK.Core.Watch;
using YUR.SDK.Core.Enums;

namespace YUR.SDK.Core.AppData
{
    /// <summary>
    /// Sets the Scale from the Server
    /// </summary>
    public class YUR_Scale : DataSetter
    {
        private Transform m_transform = null;

        public override void Awake()
        {
            base.Awake();

            m_transform = transform;
        }
        protected override string DefaultDataTag => "scale";

        public override void ApplyOverlayUpdate(OverlayStatusUpdate OSU)
        {
            base.ApplyOverlayUpdate(OSU);

            float[] arbArray = OSU.WidgetAppData?.TryGetPropertyPath<float[]>(PropertyPath);

            try
            {
                m_transform.localScale = YUR_Formatter.ConvertAs(arbArray, YurFormat.Vector3);
            }
            catch (Exception e)
            {
                YUR_Manager.Instance.Log($"Could not get update because {e.Message}");
                YUR_Manager.Instance.OnYURUpdate.RemoveListener(ApplyOverlayUpdate);
            }
        }
    } 
}
