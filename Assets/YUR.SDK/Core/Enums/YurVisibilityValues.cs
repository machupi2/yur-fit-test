namespace YUR.SDK.Core.Enums
{
	public enum YurVisibilityValues
	{
		HeartRateAvailable,
		WorkoutInProgress,
		BioDataValid,
		LoggedIn,
		Online,
		NotificationActive,
		NotificationHeaderActive,
		NotificationBodyActive,
		NotificationFooterActive,
		TwentyFourHourTime,
		DayLevel,
		IsFace
	}
}

/*
[YurVisibilityValues]:
"HeartRateAvailable":	visible if a heart rate monitor heart rate is available
"WorkoutInProgress":	visible if a workout is in progress
"BioDataValid":			visible if biodata is valid
"LoggedIn":				visible if logged in
"Online":				visible if online
"NotificationActive":	visible if a notification is active
"NotificationHeaderActive": visible if a notification that has a header is active
"NotificationBodyActive":   visible if a notification that has a body is active
"NotificationFooterActive":   visible if a notification that has a footer is active
"24HourTime":               visible if the current locale uses 24 hour time
"DayLevel=X":               visible if the day's circle level is X (0-4)
"IsFace":				visible if the being placed on the Face of the layout
*/
