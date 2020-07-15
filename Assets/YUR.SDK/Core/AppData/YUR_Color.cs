using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YUR.Fit.Core.Models;
using YUR.SDK.Core.Initialization;
using YUR.SDK.Core.Utils;
using YUR.SDK.Core.Watch;

namespace YUR.SDK.Core.AppData
{
    /// <summary>
    /// Sets the Color from the Server
    /// </summary>
    public class YUR_Color : DataSetter
    {
        private Image m_image = null;
        private TMP_Text m_text = null;
        private Renderer m_rend = null;

        public override void Awake()
        {
            base.Awake();

            switch (TargetedObject)
            {
                case ObjectToTarget.Image:
                    m_image = GetComponent<Image>();
                    break;
                case ObjectToTarget.Text:
                    m_text = GetComponent<TMP_Text>();
                    break;
                case ObjectToTarget.Renderer:
                    m_rend = GetComponent<Renderer>();
                    break;
                default:
                    break;
            }
        }

        protected override string DefaultDataTag => "color";

        public override void ApplyOverlayUpdate(OverlayStatusUpdate OSU)
        {
            base.ApplyOverlayUpdate(OSU);

            string arbString = OSU.WidgetAppData?.TryGetPropertyPath<string>(PropertyPath);

            if (!string.IsNullOrEmpty(arbString))
            {
                try
                {
                    switch (TargetedObject)
                    {
                        case ObjectToTarget.Image:
                            m_image.material.color = YUR_Formatter.ConvertToColor(arbString);
                            break;
                        case ObjectToTarget.Text:
                            m_text.fontMaterial.color = YUR_Formatter.ConvertToColor(arbString);
                            break;
                        case ObjectToTarget.Renderer:
                            m_rend.material.SetColor("_Color", YUR_Formatter.ConvertToColor(arbString));
                            break;
                        default:
                            break;
                    }
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