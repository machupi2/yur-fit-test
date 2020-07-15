using System.Collections.Generic;
using UnityEngine;

namespace YUR.SDK.Core.Utils
{
    public static class ColorFunctions
    {
        public static string DEFAULT_WIDGET_BG_COLOR = "FF444444";

        public static readonly Dictionary<int, string> DAY_LEVELS = new Dictionary<int, string>()
    {
            { 0, "FF444444" },
            { 1, "FF18B14C" },
            { 2, "FF2885EB" },
            { 3, "FF792EEB" },
            { 4, "FFff701c" }
    };

        public static readonly Dictionary<int, string> ActivityLevelColors = new Dictionary<int, string>()
    {
            { 0, "FF999999" },
            { 1, "FF18B14C" },
            { 2, "FF2885EB" },
            { 3, "FF792EEB" },
            { 4, "FFff701c" }
    };

        public static Color GetDefaultColor()
        {
            Color colorToReturn;
            if (ColorUtility.TryParseHtmlString(DEFAULT_WIDGET_BG_COLOR, out colorToReturn))
            {
                return colorToReturn;
            }
            else
            {
                return Color.grey;
            }
        }

        public static Color GetDayLevelColor(int index)
        {
            Color colorToReturn;
            if (ColorUtility.TryParseHtmlString(DAY_LEVELS[index], out colorToReturn))
            {
                return colorToReturn;
            }
            else
            {
                return GetDefaultColor();
            }
        }

        public static Color GetTodayCalsColor(float todayCals)
        {
            var level = (int)Mathf.Floor(todayCals / 250);

            if (level > 4)
                level = 4;
            else if (level < 0)
                level = 0;

            Color colorToReturn;
            if (ColorUtility.TryParseHtmlString(DAY_LEVELS[level], out colorToReturn))
            {
                return colorToReturn;
            }
            else
            {
                return GetDefaultColor();
            }
        }

        public static Color GetWorkoutTimeColor(int seconds)
        {
            int level = 0;
            //arbitrary values, 10 minutes green, 20 minutes blue, 30 minutes purple, 40 minutes orange
            if (seconds >= 2400)
            {
                level = 4;
            }
            else if (seconds >= 1800)
            {
                level = 3;
            }
            else if (seconds >= 1200)
            {
                level = 2;
            }
            else if (seconds >= 600)
            {
                level = 1;
            }

            Color colorToReturn;
            if (ColorUtility.TryParseHtmlString(DAY_LEVELS[level], out colorToReturn))
            {
                return colorToReturn;
            }
            else
            {
                return GetDefaultColor();
            }
        }

        public static Color GetTodayTimeColor(int seconds)
        {
            int level = 0;
            //arbitrary values, 20 minutes green, 40 minutes blue, 60 minutes purple, 120 minutes orange
            if (seconds >= 7200)
            {
                level = 4;
            }
            else if (seconds >= 3600)
            {
                level = 3;
            }
            else if (seconds >= 2400)
            {
                level = 2;
            }
            else if (seconds >= 1200)
            {
                level = 1;
            }

            Color colorToReturn;
            if (ColorUtility.TryParseHtmlString(DAY_LEVELS[level], out colorToReturn))
            {
                return colorToReturn;
            }
            else
            {
                return GetDefaultColor();
            }
        }

        public static Color GetSquatColor(float squatCount)
        {
            const int maxSquat = 100;
            int index = 0;
            if (squatCount >= maxSquat)
            {
                index = 4;
            }
            else if (squatCount >= (maxSquat * 0.75F))
            {
                index = 3;
            }
            else if (squatCount >= (maxSquat * 0.5F))
            {
                index = 2;
            }
            else if (squatCount >= (maxSquat * 0.25F))
            {
                index = 1;
            }
            else
            {
                index = 0;
            }

            Color colorToReturn;
            if (ColorUtility.TryParseHtmlString(DAY_LEVELS[index], out colorToReturn))
            {
                return colorToReturn;
            }
            else
            {
                return GetDefaultColor();
            }
        }

        /// <summary>
        /// Determine if a color should have black or white text in front of it
        /// </summary>
        /// <param name="hexColor"></param>
        /// <returns></returns>
        public static Color GetReadableForeColor(string hexColor)
        {
            if (hexColor.StartsWith("#"))
            {
                hexColor = hexColor.Substring(1);
            }
            System.Globalization.NumberStyles hexToDecLum = System.Globalization.NumberStyles.AllowHexSpecifier;
            int r = int.Parse(hexColor.Substring(0, 2), hexToDecLum);
            int g = int.Parse(hexColor.Substring(2, 2), hexToDecLum);
            int b = int.Parse(hexColor.Substring(4, 2), hexToDecLum);

            var perceivedBright = (int)Mathf.Sqrt((r * r * .241f) + (g * g * .691f) + (b * b * .068f));

            return (perceivedBright > 175) ? Color.black : Color.white;
        }
    } 
}
