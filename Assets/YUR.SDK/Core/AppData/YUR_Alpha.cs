using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YUR.Fit.Core.Models;
using YUR.SDK.Core.Initialization;
using YUR.SDK.Core.Watch;

namespace YUR.SDK.Core.AppData
{
    /// <summary>
    /// Sets the Alpha from the Server
    /// </summary>
    public class YUR_Alpha : DataSetter
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
        protected override string DefaultDataTag => "alpha";

        public override void ApplyOverlayUpdate(OverlayStatusUpdate OSU)
        {
            base.ApplyOverlayUpdate(OSU);

            float? arbFloat = OSU.WidgetAppData?.TryGetPropertyPath<float>(PropertyPath);

            if (arbFloat != null)
            {
                Color col = Color.white;

                try
                {
                    switch (TargetedObject)
                    {

                        case ObjectToTarget.Image:
                            col = m_image.color;
                            col.a = (float)arbFloat;
                            m_image.color = col;
                            break;
                        case ObjectToTarget.Text:
                            col = m_text.fontMaterial.color;
                            col.a = (float)arbFloat;
                            m_text.fontMaterial.color = col;
                            break;
                        case ObjectToTarget.Renderer:
                            col = m_rend.material.color;
                            col.a = (float)arbFloat;
                            m_rend.material.SetColor("_Color", col);
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
