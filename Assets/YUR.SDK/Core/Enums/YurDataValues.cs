namespace YUR.SDK.Core.Enums
{
    public enum YurDataValues
    {
        TagValue,
        ClockTime,
        ClockTimeSuffix,
        SquatCount,
        EstOrHeartRate,
        HeartRate,
        EstRate,
        BurnRate,
        TodayCalories,
        TodaySquatCount,
        TodayTime,
        TodayTimeNoSeconds,
        WorkoutTime,
        WorkoutTimeNoSeconds,
        WorkoutCalories,
        UserRank,
        LocalTime,
        NotificationHeader,
        NotificationBody,
        NotificationFooter,
        TotalXP,
        NextLevelXP,
        CurrentLevelXP,
        XPCompletionPercentage
    }
}

/*
[YurDataValues]:
"tag value" (string format): description
"ClockTime":    The clock time, sans-suffix (AM/PM)
"ClockTimeSuffix": The clock time suffix, if not in 24h mode
"SquatCount" (N0):	The number of squats for the current workout 
"EstOrHeartRate" (N0):	The actual heart rate if it is available, otherwise the estimated heart rate 
"HeartRate" (N0):	The actual heart rate (will be 0 if unavailable)
"EstRate" (N0):		The estimated heart rate
"BurnRate" (0.#):	The current burn rate in kcal/min	
"TodayCalories" (N0): The total number of calories for the current day
"TodaySquatCount" (N0):	The number of squats for the day
"TodayTime" (hh:mm:ss): The total time of all workouts for the day
"TodayTimeNoSeconds" (hh:mm): The total time of all workouts for the day
"WorkoutTime" (hh:mm:ss): The total time of the current workout
"WorkoutTimeNoSeconds" (hh:mm): The total time of the current workout
"WorkoutCalories" (N0):  The number of calories burned in the current workout
"UserRank" (N0): The user's current level
"LocalTime" (HH:mm or h:mm\ntt): The current time (as in a clock)
"NotificationHeader":   The header text of an active notification with a header
"NotificationBody":   The body text of an active notification with a body
"NotificationFooter":   The footer text of an active notification with a footer
"TotalXP" (short rounded): The total XP that the user has, formated shortly (e.g. "12k" or "1.3k" or "100" or "1.5")
"NextLevelXP" (short rounded): The total XP that the user has, formated shortly (e.g. "12k" or "1.3k" or "100" or "1.5")
"CurrentLevelXP" (short rounded): The total XP that the user has, formated shortly (e.g. "12k" or "1.3k" or "100" or "1.5")
*/