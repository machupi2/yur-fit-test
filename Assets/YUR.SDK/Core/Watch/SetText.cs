using System;
using YUR.Fit.Core.Models;
using YUR.SDK.Core.Enums;
using YUR.SDK.Core.Utils;
using TMPro;
using YUR.SDK.Core.Initialization;
using UnityEngine;
using YUR.SDK.Core.Interfaces;

namespace YUR.SDK.Core.Watch
{
    public class SetText : MonoBehaviour, IYURUpdatable
    {
        public YurDataValues TextValue;

        private TMP_Text m_text = null;

        private void Awake()
        {
            m_text = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            if (gameObject.GetComponent<SetFont>() == null)
            {
                gameObject.AddComponent(typeof(SetFont));
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
            try
            {
                if (m_text.enabled)
                {
                    m_text.SetText(GetTextTagValue(OSU, TextValue));
                }
                    
            } catch (Exception e)
            {
                if (m_text == null)
                {
                    m_text = GetComponent<TMP_Text>();
                }

                YUR_Manager.Instance.Log($"Could not get update because {e.Message}");
            }
        }

        public static string GetTextTagValue(OverlayStatusUpdate info, YurDataValues tag)
        {
            string value = null;

            switch (tag)
            {
                case YurDataValues.TodaySquatCount:
                    value = YUR_Formatter.ConvertAs(info.TodaySquatCount, YurFormat.NoSigValues);
                    break;
                case YurDataValues.SquatCount:
                    value = YUR_Formatter.ConvertAs(info.SquatCount, YurFormat.NoSigValues);
                    break;
                case YurDataValues.EstOrHeartRate:
                    value = YUR_Formatter.ConvertAs((info.HeartRate ?? 0) > 0? info.HeartRate.Value : info.CalculationMetrics.EstHeartRate, YurFormat.NoSigValues);
                    break;
                case YurDataValues.HeartRate:
                    value = YUR_Formatter.ConvertAs((info.HeartRate ?? 0), YurFormat.NoSigValues);
                    break;
                case YurDataValues.TotalXP:
                    value = YUR_Formatter.ConvertAs(info.TotalXP, YurFormat.NoSigValues);
                    break;
                case YurDataValues.NextLevelXP:
                    value = YUR_Formatter.ConvertAs((info.RankNextRequiredXP <= info.RankCurrentRequiredXP)? info.RankCurrentRequiredXP : info.RankNextRequiredXP, YurFormat.ShortK);
                    break;
                case YurDataValues.CurrentLevelXP:
                    value = YUR_Formatter.ConvertAs(info.RankCurrentRequiredXP, YurFormat.ShortK);
                    break;
                case YurDataValues.EstRate:
                    value = YUR_Formatter.ConvertAs(info.CalculationMetrics.EstHeartRate, YurFormat.NoSigValues);
                    break;
                case YurDataValues.BurnRate:
                    value = YUR_Formatter.ConvertAs(info.CalculationMetrics.EstHeartRate, YurFormat.OneSigValue);
                    break;
                case YurDataValues.TodayCalories:
                    value = YUR_Formatter.ConvertAs(info.TodayCalories, YurFormat.NoSigValues);
                    break;
                case YurDataValues.TodayTime:
                    value = YUR_Formatter.ConvertAs(info.SecondsToday, YurFormat.Time);
                    break;
                case YurDataValues.TodayTimeNoSeconds:
                    value = YUR_Formatter.ConvertAs(info.SecondsToday, YurFormat.TimeNoSeconds);
                    break;
                case YurDataValues.WorkoutTime:
                    value = YUR_Formatter.ConvertAs(info.SecondsInWorkout, YurFormat.Time);
                    break;
                case YurDataValues.WorkoutTimeNoSeconds:
                    value = YUR_Formatter.ConvertAs(info.SecondsInWorkout, YurFormat.TimeNoSeconds);
                    break;
                case YurDataValues.WorkoutCalories:
                    value = YUR_Formatter.ConvertAs(info.CurrentCalories, YurFormat.NoSigValues);
                    break;
                case YurDataValues.UserRank:
                    value = YUR_Formatter.ConvertAs(info.Rank, YurFormat.NoSigValues);
                    break;
                case YurDataValues.ClockTime:
                    value = YUR_Formatter.ConvertAs(DateTime.Now.ToString(), YurFormat.FormattedTime);
                    break;
                case YurDataValues.ClockTimeSuffix:
                    value = YUR_Formatter.ConvertAs(DateTime.Now.ToString(), YurFormat.TimeOfDaySignifier);
                    break;
                case YurDataValues.XPCompletionPercentage:
                    float nrankDiff = info.RankNextRequiredXP - info.RankCurrentRequiredXP;
                    float caltonrank = info.RankNextRequiredXP - info.TotalXP;
                    value = YUR_Formatter.ConvertAs((1 - (caltonrank / nrankDiff)), YurFormat.TwoSigValues);
                    break;
                default:
                    //unknown
                    value = null;
                    break;
            }

            return value;
        }
    } 
}
