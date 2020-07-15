namespace YUR.SDK.Core.Enums
{
    public enum YurFillColors
    {
        Default,
        SquatCountLevel,
        TodaySquatCountLevel,
        EstOrHeartRateLevel,
        BurnRateLevel,
        TodayCaloriesLevel,
        UserRank,
        UserRankContrast,
        WorkoutCaloriesLevel,
        WorkoutTimeLevel,
        TodayTimeLevel
    }
}

/*
[YurFillColors]:
"SquatCountLevel":		The rarity color based on the squat count for the workout
"TodaySquatCountLevel":	The rarity color based on the squat count for the day
"EstOrHeartRateLevel":		The rarity color based on the heart rate relative to the user's max heart rate (i.e. activity level)
"BurnRateLevel":			The rarity color based on the current activity level band
"TodayCaloriesLevel":		The rarity color based on calorie activity for the day
"UserRank":					The color for the user's current rank
"UserRankContrast":		A color that contrasts well with the user's current rank (e.g. text color for being used on a background of UserRank color)
"WorkoutCaloriesLevel":	The rarity color based on the current workout's calories (although, not sure what this is)
"WorkoutTimeLevel":     The rarity color based on the time for the workout
"TodayTimeLevel":     The rarity color based on the time for the entire day
*/
