using UnityEngine;
using TMPro;
using System;
using YUR.SDK.Core.Initialization;

namespace YUR.SDK.Core.Watch
{
    public class SetDownloadText : MonoBehaviour
    {
        private TMP_Text m_text = null;

        private void Awake()
        {
            m_text = GetComponent<TMP_Text>();
        }

        public void UpdateProgress(Single progress, Single maxCount)
        {
            try
            {
                m_text.text = "Downloading " + progress.ToString() + "/" + maxCount;
            }
            catch (Exception e)
            {

                YUR_Manager.Instance.Log($"Couldn't get download text because {e.Message}");
            }
        }
    } 
}
