using System;
using UnityEngine;
using YUR.Fit.Core.Watch;
using YUR.SDK.Core.Initialization;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Retrieves slots open on the sleeve
    /// </summary>
    public class SetWidgetPlacements : MonoBehaviour
    {
        [SerializeField] private GameObject[] _placementSlots = null;
        [SerializeField] private GameObject _placementContainer = null;

        public void TogglePlacements(bool activeState)
        {
            try
            {
                foreach (GameObject placement in _placementSlots)
                {
                    try
                    {
                        if (placement.name != "Face")
                        {
                            placement.transform.GetChild(0).gameObject.SetActive(activeState);
                        }
                    }
                    catch (Exception e)
                    {
                        YUR_Manager.Instance.Log($"Could not get toggle placements because {e.Message}");
                    }
                }
            } catch (Exception e)
            {
                YUR_Manager.Instance.Log($"Could not get toggle placements because {e.Message}");
            }
           
            _placementContainer.SetActive(activeState);
        }

        internal Transform GetOpenSlot()
        {
            Transform openSlot = null;

            foreach (GameObject _placementSlot in _placementSlots)
            {
                if (_placementSlot.transform.childCount == 0)
                {
                    openSlot = _placementSlot.transform;
                    break;
                }
            }

            return openSlot;
        }

        internal Transform GetSlot(WidgetPosition widgetEnum)
        {
            Transform slot = null;

            foreach(GameObject placement in _placementSlots)
            {
                if (placement.name == widgetEnum.ToString())
                {
                    slot = placement.transform;
                }
            }

            return slot;
        }

        public void ClearWidgets()
        {
            foreach (GameObject placement in _placementSlots)
            {
                if (placement.transform.childCount > 0)
                {
                    for (int i = 0; i < placement.transform.childCount; i++)
                    {
                        Destroy(placement.transform.GetChild(i).gameObject);
                    }
                }
            }
        }
    } 
}
