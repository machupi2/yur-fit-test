using System;
using TMPro;
using YUR.Fit.Core.Models;
using YUR.SDK.Core.Initialization;
using YUR.SDK.Core.Watch;

namespace YUR.SDK.Core.AppData
{
    /// <summary>
    /// Sets the Font Size from the Server
    /// </summary>
    public class YUR_FontSize : DataSetter
    {
        private TMP_Text m_text = null;

        public override void Awake()
        {
            base.Awake();

            m_text = GetComponent<TMP_Text>();
        }

        protected override string DefaultDataTag => "fontsize";

        public override void ApplyOverlayUpdate(OverlayStatusUpdate OSU)
        {
            base.ApplyOverlayUpdate(OSU);

            int? arbInt = OSU.WidgetAppData?.TryGetPropertyPath<int>(PropertyPath);

            if (arbInt != null)
            {
                try
                {
                    m_text.fontSize = (int)arbInt;
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
