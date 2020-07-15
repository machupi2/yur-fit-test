using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YUR.SDK.Core.Enums;
using YUR.SDK.Core.Initialization;
using YUR.SDK.Core.Interfaces;

namespace YUR.SDK.Core.Utils
{
    public static class YUR_Formatter
    {
        internal static Vector3 ConvertAs(float[] param, YurFormat formatType)
        {
            Vector3 returnObject = Vector3.zero;

            switch (formatType)
            {
                case YurFormat.None:
                    break;
                case YurFormat.Vector3:
                    returnObject = ConvertToVector3(param);
                    break;
                default:
                    break;
            }

            return returnObject;
        }

        internal static string ConvertAs(string param, YurFormat formatType)
        {
            string returnObject = null;

            switch (formatType)
            {
                case YurFormat.None:
                    break;
                case YurFormat.ShortK:
                    returnObject = ConvertToShortK(param);
                    break;
                case YurFormat.Time:
                    returnObject = ConvertToTime(param);
                    break;
                case YurFormat.TimeNoSeconds:
                    returnObject = ConvertToTimeNoSeconds(param);
                    break;
                case YurFormat.FormattedTime:
                    returnObject = ConvertToFormattedTime(param);
                    break;
                case YurFormat.TimeOfDaySignifier:
                    returnObject = ConvertToTimeOfDaySignifier(param);
                    break;
                default:
                    break;
            }

            return returnObject.ToString();
        }

        internal static string ConvertAs(int param, YurFormat formatType)
        {
            string returnObject = null;

            switch (formatType)
            {
                case YurFormat.None:
                    returnObject = param.ToString();
                    break;
                case YurFormat.NoSigValues:
                    returnObject = ConvertToStringFromInt(param, "N0");
                    break;
                case YurFormat.OneSigValue:
                    returnObject = ConvertToStringFromInt(param, "0.#");
                    break;
                case YurFormat.TwoSigValues:
                    returnObject = ConvertToStringFromInt(param, "F2");
                    break;
                case YurFormat.Time:
                    returnObject = ConvertToTimeFromInt(param);
                    break;
                default:
                    returnObject = param.ToString();
                    break;
            }

            return returnObject.ToString();
        }

        internal static string ConvertAs(float param, YurFormat formatType)
        {
            string returnObject = null;

            switch (formatType)
            {
                case YurFormat.None:
                    returnObject = param.ToString();
                    break;
                case YurFormat.NoSigValues:
                    returnObject = ConvertToStringFromFloat(param, "N0");
                    break;
                case YurFormat.OneSigValue:
                    returnObject = ConvertToStringFromFloat(param, "0.#");
                    break;
                case YurFormat.TwoSigValues:
                    returnObject = ConvertToStringFromFloat(param, "F2");
                    break;
                default:
                    returnObject = param.ToString();
                    break;
            }

            return returnObject.ToString();
        }

        public static string ConvertToTimeFromInt(int param)
        {
            int minutes = 0;
            int seconds = param;

            int modulo = param / 60;

            if (modulo > 0)
            {
                minutes = modulo;
            }

            while (seconds > 59)
            {
                seconds -= 60;
            }

            string fmtSec = seconds.ToString();

            if(fmtSec.Length == 1)
            {
                fmtSec = seconds.ToString().PadLeft(2, '0');
            }

            return $"{minutes}:{fmtSec}";
        }

        public static Color ConvertToColor(string obj)
        {
            Color color = Color.grey;

            if (ColorUtility.TryParseHtmlString(obj, out Color col))
            {
                color = col;
            }

            return color;
        }

        public static Vector3 ConvertToVector3(float[] obj)
        {
            Vector3 v3 = Vector3.zero;

            try
            {
                // Fill in parsed values into Vector3 object
                v3 = new Vector3
                (
                    obj[0],
                    obj[1],
                    obj[2]
                );
            } catch (UnityException e)
            {
                YUR_Manager.Instance.Log($"Could not get conversion because {e.Message}");
            }

            return v3;
        }

        public static Quaternion ConvertToQuaternion(float[] obj)
        {
            Quaternion rot = Quaternion.identity;

            try
            {
                // Fill in parsed values into Quaternion object
                rot = new Quaternion
                (
                    obj[0],
                    obj[1],
                    obj[2],
                    obj[3]
                );
            }
            catch (UnityException e)
            {
                YUR_Manager.Instance.Log($"Could not get conversion because {e.Message}");
            }

            return rot;
        }

        public static string ConvertToStringFromInt(int obj, string formatParams = null)
        {
            return obj.ToString(formatParams);
        }

        public static string ConvertToStringFromFloat(float obj, string formatParams = null)
        {
            return obj.ToString(formatParams);
        }

        public static string ConvertToShortK(string obj)
        {
            float number = float.Parse(obj as string);

            if (number >= 10000)
            {
                return (number / 1000).ToString("N0") + "k";
            }
            else if (number >= 1000)
            {
                return (number / 1000).ToString("0.#") + "k";
            }
            else if (number >= 100)
            {
                return number.ToString("N0");
            }
            else
            {
                return number.ToString("0.#");
            }
        }

        public static string ConvertToTime(string obj)
        {
            int seconds = int.Parse(obj as string);

            int secs = seconds;
            int hours = 0;
            int mins = 0;
            if (secs > 3600)
            {
                hours = secs / 3600;
                secs = secs % 3600;
            }

            if (secs > 60)
            {
                mins = secs / 60;
                secs = secs % 60;
            }

            string output = "";
            if (hours > 0)
            {
                output = $"{hours}:{mins:D2}:{secs:D2}";
            }
            else
            {
                output = $"{mins}:{secs:D2}";
            }
            return output;
        }

        public static string ConvertToTimeNoSeconds(string obj)
        {
            int seconds = int.Parse(obj as string);

            int secs = seconds;
            int hours = 0;
            int mins = 0;
            if (secs > 3600)
            {
                hours = secs / 3600;
                secs = secs % 3600;
            }

            if (secs > 60)
            {
                mins = secs / 60;
                secs = secs % 60;
            }

            string output = "";
            if (hours > 0)
            {
                output = $"{hours}:{mins:D2}";
            }
            else
            {
                output = $"0:{mins}";
            }
            return output;
        }

        public static string ConvertToFormattedTime(string obj)
        {
            DateTime date = DateTime.Parse(obj as string);

            string output = "";

            output = date.ToString(System.Threading.Thread.CurrentThread.CurrentCulture
                .DateTimeFormat.ShortTimePattern.Contains("H") ? "H:mm" : "h:mm");

            return output;
        }

        public static string ConvertToTimeOfDaySignifier(string obj)
        {
            DateTime date = DateTime.Parse(obj as string);

            string output = "";

            output = System.Threading.Thread.CurrentThread.CurrentCulture
                .DateTimeFormat.ShortTimePattern.Contains("H") ? "" : date.ToString("tt");

            return output;
        }
    }
}
