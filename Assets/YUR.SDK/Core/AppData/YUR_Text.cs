using System;
using TMPro;
using YUR.Fit.Core.Models;
using YUR.SDK.Core.Initialization;
using YUR.SDK.Core.Utils;
using YUR.SDK.Core.Watch;
using YUR.SDK.Core.Enums;
using YUR.Fit.Core.Services;

namespace YUR.SDK.Core.AppData
{
    /// <summary>
    /// Sets the Text from the Server
    /// </summary>
    public class YUR_Text : DataSetter
    {
        public YurFormat format = YurFormat.None;
        private TMP_Text m_text = null;

        public override void Awake()
        {
            base.Awake();

            m_text = GetComponent<TMP_Text>();
        }

        protected override string DefaultDataTag => "text";

        public override void ApplyOverlayUpdate(OverlayStatusUpdate OSU)
        {
            base.ApplyOverlayUpdate(OSU);

            object appObj = OSU.WidgetAppData?.TryGetPropertyPath<object>(PropertyPath);

            NewtonsoftSerializer s = new NewtonsoftSerializer();
            print(s.SerializeObject(OSU.WidgetAppData));

            print(appObj.GetType());

            if (appObj != null)
            {
                print(appObj.GetType());

                string formatted = "";
                if (appObj != null)
                {
                    if (appObj.GetType() == typeof(string))
                    {
                        m_text.SetText((string)appObj);
                    }
                    else
                    {
                        float convertedFloat = (float)Convert.ChangeType(appObj, typeof(float));

                        try
                        {

                            m_text.SetText(YUR_Formatter.ConvertAs(convertedFloat, format));
                        }
                        catch
                        {
                            formatted = "";
                        }
                    }
                }
            }
        }
    }
}
