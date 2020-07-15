using UnityEngine;
using UnityEngine.UI;
using YUR.Fit.Core.Models;
using YUR.SDK.Core.Enums;
using YUR.SDK.Core.Initialization;
using YUR.SDK.Core.Interfaces;

namespace YUR.SDK.Core.Watch
{
    public class SetPercentageFill : MonoBehaviour, IYURUpdatable
    {
        public YurDataValues DataValue = YurDataValues.TodayCalories;

        [Header("NOTE: Overrided if Data Value includes a floor/ceiling")]
        public float Floor = 0;
        public float Ceiling = 1;

        private Image m_image = null;
        private float? m_overrideFloor = 0;
        private float? m_overrideCeiling = 0;

        private void Awake()
        {
            m_image = GetComponent<Image>();
            
            // Ensure we are using a fill type image
            m_image.type = Image.Type.Filled;
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
            float val = GetDataTagValue(OSU, DataValue, out m_overrideFloor, out m_overrideCeiling);

            Floor = m_overrideFloor == null ? Floor : (float)m_overrideFloor;
            Ceiling = m_overrideCeiling == null ? Ceiling : (float)m_overrideCeiling;

            m_image.fillAmount = GetPercentage(val, Floor, Ceiling);
        }

        private float GetPercentage(float valueToCheck, float floor, float ceiling)
        {
            float fillAmount = 0.0f;

            // Handle edge case where floor is somehow higher than ceiling and vice versa
            if (floor >= ceiling)
            {
                floor = ceiling - 0.1f;
            }

            if (ceiling <= floor)
            {
                ceiling = floor + 0.1f;
            }

            // Get percentage between floor and ceiling
            if (valueToCheck >= floor && valueToCheck <= ceiling)
            {
                fillAmount =
                    Mathf.InverseLerp
                    (
                        floor,
                        ceiling,
                        valueToCheck
                    );
            }
            else if (valueToCheck >= ceiling)
            {
                fillAmount = 1;
            } else if (valueToCheck <= floor)
            {
                fillAmount = 0;
            }

            return fillAmount;
        }

        public static float GetDataTagValue(OverlayStatusUpdate info, YurDataValues tag, out float? floorValue, out float? ceilingValue)
        {
            float value = 0.0f;
            floorValue = null;
            ceilingValue = null;

            switch (tag)
            {
                case YurDataValues.TodaySquatCount:
                    value = info.TodaySquatCount;
                    break;
                case YurDataValues.SquatCount:
                    value = info.SquatCount;
                    break;
                case YurDataValues.EstOrHeartRate:
                    value = ((info.HeartRate ?? 0) > 0 ? info.HeartRate.Value : info.CalculationMetrics.EstHeartRate);
                    break;
                case YurDataValues.HeartRate:
                    value = (info.HeartRate ?? 0);
                    break;
                case YurDataValues.TotalXP:
                    value = info.TotalXP;
                    break;
                case YurDataValues.XPCompletionPercentage:
                    float nrankDiff = info.RankNextRequiredXP - info.RankCurrentRequiredXP;
                    float caltonrank = info.RankNextRequiredXP - info.TotalXP;

                    value = 1 - (caltonrank / nrankDiff);
                    break;
                case YurDataValues.NextLevelXP:
                    value = (info.RankNextRequiredXP <= info.RankCurrentRequiredXP) ? info.RankCurrentRequiredXP : info.RankNextRequiredXP;
                    break;
                case YurDataValues.CurrentLevelXP:
                    value = info.RankCurrentRequiredXP;
                    break;
                case YurDataValues.EstRate:
                    value = info.CalculationMetrics.EstHeartRate;
                    break;
                case YurDataValues.BurnRate:
                    value = info.CalculationMetrics.BurnRate;
                    break;
                case YurDataValues.TodayCalories:
                    value = info.TodayCalories;
                    break;
                case YurDataValues.TodayTime:
                    value = info.SecondsToday;
                    break;
                case YurDataValues.TodayTimeNoSeconds:
                    value = info.SecondsToday;
                    break;
                case YurDataValues.WorkoutTime:
                    value = info.SecondsInWorkout;
                    break;
                case YurDataValues.WorkoutTimeNoSeconds:
                    value = info.SecondsInWorkout;
                    break;
                case YurDataValues.WorkoutCalories:
                    value = info.CurrentCalories;
                    break;
                case YurDataValues.UserRank:
                    value = info.Rank;
                    break;
                case YurDataValues.NotificationHeader:
                    value = 0.0f;
                    break;
                case YurDataValues.NotificationBody:
                    value = 0.0f;
                    break;
                case YurDataValues.NotificationFooter:
                    value = 0.0f;
                    break;
                case YurDataValues.ClockTime:
                    value = 0.0f;
                    break;
                case YurDataValues.ClockTimeSuffix:
                    value = 0.0f;
                    break;
                default:
                    //unknown
                    value = 0.0f;
                    break;
            }

            return value;
        }
    } 
}
