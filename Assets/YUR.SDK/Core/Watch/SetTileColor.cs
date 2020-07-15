using System;
using UnityEngine;
using YUR.SDK.Core.Enums;
using YUR.SDK.Core.Initialization;

namespace YUR.SDK.Core.Watch
{
    public class SetTileColor : MonoBehaviour
    {
        public YurFillColors TileColorOverride = YurFillColors.Default;

        private void OnEnable()
        {
            try
            {
                SetFillColor SFC = transform.parent.transform.parent.GetChild(0).gameObject.GetComponent<SetFillColor>();

                if (SFC != null)
                {
                    SFC.enabled = true;
                }
            } catch (NullReferenceException e)
            {
                YUR_Manager.Instance.Log(e);
            }
        }
    } 
}
