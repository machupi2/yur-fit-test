using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YUR.Fit.Core.Models;
using YUR.SDK.Core.Enums;
using YUR.SDK.Core.Initialization;
using YUR.SDK.Core.Interfaces;
using YUR.SDK.Core.Utils;

namespace YUR.SDK.Core.Watch
{
    public enum ObjectToChangeColorOn
    {
        Renderer,
        Image,
        Text
    }

    /// <summary>
    /// 
    /// </summary>
    public class SetFillColor : MonoBehaviour, IYURUpdatable
    {
        public YurFillColors FillColor = YurFillColors.Default;
        public ObjectToChangeColorOn ObjectToTarget = ObjectToChangeColorOn.Renderer;

        private Renderer m_rend = null;
        private TMP_Text m_text = null;
        private Image m_image = null;

        private void Awake()
        {
            switch(ObjectToTarget)
            {
                case ObjectToChangeColorOn.Renderer:
                    m_rend = GetComponent<Renderer>();
                    break;
                case ObjectToChangeColorOn.Image:
                    m_image = GetComponent<Image>();
                    break;
                case ObjectToChangeColorOn.Text:
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
            switch (ObjectToTarget)
            {
                case ObjectToChangeColorOn.Renderer:
                    m_rend.material.color = GetFillColorTagValue(OSU, FillColor);
                    break;
                case ObjectToChangeColorOn.Image:
                    m_image.color = GetFillColorTagValue(OSU, FillColor);
                    break;
                case ObjectToChangeColorOn.Text:
                    m_text.color = GetFillColorTagValue(OSU, FillColor);
                    break;
                default:
                    break;
            }
        }

        public static Color GetFillColorTagValue(OverlayStatusUpdate info, YurFillColors tag)
        {
            Color value = Color.gray;
            switch (tag)
            {
                case YurFillColors.Default:
                    value = ColorFunctions.GetDefaultColor();
                    break;
                case YurFillColors.SquatCountLevel:
                    value = ColorFunctions.GetSquatColor(info.SquatCount);
                    break;
                case YurFillColors.TodaySquatCountLevel:
                    value = ColorFunctions.GetSquatColor(info.TodaySquatCount);
                    break;
                case YurFillColors.EstOrHeartRateLevel:
                    value = ColorFunctions.GetDayLevelColor((int)info.CalculationMetrics.ActivityLevel);
                    break;
                case YurFillColors.WorkoutTimeLevel:
                    value = ColorFunctions.GetWorkoutTimeColor(info.SecondsInWorkout);
                    break;
                case YurFillColors.TodayTimeLevel:
                    value = ColorFunctions.GetTodayTimeColor(info.SecondsToday);
                    break;
                case YurFillColors.BurnRateLevel:
                    value = ColorFunctions.GetDayLevelColor((int)info.CalculationMetrics.ActivityLevel);
                    break;
                case YurFillColors.TodayCaloriesLevel:
                    value = ColorFunctions.GetTodayCalsColor(info.TodayCalories);
                    break;
                case YurFillColors.UserRank:
                    Color colorToReturn;
                    if (ColorUtility.TryParseHtmlString(info.RankCurrentColor, out colorToReturn))
                    {
                        value = colorToReturn;
                    }
                    else
                    {
                        value = ColorFunctions.GetDefaultColor();
                    }
                    break;
                case YurFillColors.UserRankContrast:
                    value = ColorFunctions.GetReadableForeColor(info.RankCurrentColor);
                    break;
                case YurFillColors.WorkoutCaloriesLevel:
                    //TODO: what should this be?
                    value = ColorFunctions.GetTodayCalsColor(info.CurrentCalories);
                    break;
                default:
                    //unknown
                    value = ColorFunctions.GetDefaultColor();
                    break;
            }
            return value;
        }
    } 
}
