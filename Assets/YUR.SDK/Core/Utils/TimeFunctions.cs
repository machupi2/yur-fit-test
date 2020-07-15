namespace YUR.SDK.Core.Utils
{
	public static class TimeFunctions
	{
        public static string FormatShortK(float number)
        {
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

        public static string FormatTime(int seconds)
        {
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

        public static string FormatTimeNoSeconds(int seconds)
        {
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
    } 
}
