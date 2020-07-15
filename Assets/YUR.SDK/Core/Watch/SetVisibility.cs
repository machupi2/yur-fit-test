using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YUR.Fit.Core.Models;
using YUR.SDK.Core.Enums;
using YUR.SDK.Core.Initialization;
using YUR.SDK.Core.Interfaces;

namespace YUR.SDK.Core.Watch
{
    public enum ObjectToToggle
    {
        GameObject,
        Image,
        Text
    }

    public class SetVisibility : MonoBehaviour, IYURUpdatable
    {
        public YurVisibilityValues Tag = YurVisibilityValues.DayLevel;
        public ObjectToToggle ToggleObject = ObjectToToggle.GameObject;
        public bool turnOnIfFalse = false;

        [Header("This value is ignored if not applicable")]
        public int ValueToCheck = 0;

        private static GameObject m_object = null;
        private Image m_image = null;
        private TMP_Text m_text = null;

        private void Awake()
        {
            m_object = gameObject;

            switch (ToggleObject)
            {
                case ObjectToToggle.Image:
                    m_image = GetComponent<Image>();
                    break;
                case ObjectToToggle.Text:
                    m_text = GetComponent<TMP_Text>();
                    break;
                default:
                    break;
            }
        }

        private void OnEnable()
        {
            YUR_Manager.Instance.OnYURUpdate.AddListener(ApplyOverlayUpdate);
        }

        private void OnDisable()
        {
            YUR_Manager.Instance.OnYURUpdate.RemoveListener(ApplyOverlayUpdate);
        }

        public void ApplyOverlayUpdate(OverlayStatusUpdate OSU)
        {
            m_object = gameObject;

            try
            {
                switch (ToggleObject)
                {
                    case ObjectToToggle.GameObject:
                        gameObject.SetActive(GetElementVisibilityFromTag(OSU, Tag, ValueToCheck, turnOnIfFalse));
                        break;
                    case ObjectToToggle.Image:
                        m_image.enabled = GetElementVisibilityFromTag(OSU, Tag, ValueToCheck, turnOnIfFalse);
                        break;
                    case ObjectToToggle.Text:
                        m_text.enabled = GetElementVisibilityFromTag(OSU, Tag, ValueToCheck, turnOnIfFalse);
                        break;
                    default:
                        break;
                }
            } catch (Exception e)
            {
                YUR_Manager.Instance.Log($"Could not update object because {e.Message}");
                YUR_Manager.Instance.OnYURUpdate.RemoveListener(ApplyOverlayUpdate);
            }
        }

        public static bool GetElementVisibilityFromTag(OverlayStatusUpdate info, YurVisibilityValues tag, int equalsVal, bool reverseResult)
        {
            bool? value;

            switch (tag)
            {
                case YurVisibilityValues.DayLevel:
                    if (equalsVal <= 0)
                    {
                        value = false;
                    }
                    else
                    {
                        var calLvl = Mathf.Max(Mathf.Min((int)info.TodayCalories / 250, 4), 0);
                        value = calLvl == equalsVal;
                    }
                    break;
                case YurVisibilityValues.HeartRateAvailable:
                    value = ((info.HeartRate ?? 0) >= 10);
                    break;
                case YurVisibilityValues.WorkoutInProgress:
                    value = info.IsWorkoutInProgress;
                    break;
                case YurVisibilityValues.BioDataValid:
                    value = info.IsBioValid;
                    break;
                case YurVisibilityValues.LoggedIn:
                    value = info.IsLoggedIn;
                    break;
                case YurVisibilityValues.Online:
                    value = info.IsOnline;
                    break;
                case YurVisibilityValues.NotificationActive:
                    value = info.CurrentNotification != null;
                    break;
                case YurVisibilityValues.NotificationHeaderActive:
                    value = ((info.CurrentNotification?.Type ?? 0) & NotificationType.Header) != 0;
                    break;
                case YurVisibilityValues.NotificationBodyActive:
                    value = ((info.CurrentNotification?.Type ?? 0) & NotificationType.Body) != 0;
                    break;
                case YurVisibilityValues.NotificationFooterActive:
                    value = ((info.CurrentNotification?.Type ?? 0) & NotificationType.Footer) != 0;
                    break;
                case YurVisibilityValues.TwentyFourHourTime:
                    value = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Contains("H");
                    break;
                case YurVisibilityValues.IsFace:
                    value = string.Equals(m_object.transform.parent.transform.parent.name, "Face");
                    break;
                default:
                    value = false;
                    break;
            }

            if (!value.HasValue)
            {
                //hmm... default to visible? Gir's response: Nahhhhh
                value = false;
            }
            if (reverseResult)
            {
                value = !value.Value;
            }
            return value.Value;
        }
    } 
}
