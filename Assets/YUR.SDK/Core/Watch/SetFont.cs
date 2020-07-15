using TMPro;
using UnityEngine;
using YUR.SDK.Core.Initialization;

namespace YUR.SDK.Core.Watch
{
    public class SetFont : MonoBehaviour
    {
        private TextMeshProUGUI m_font = null;
        private TMP_Text m_text = null;

        private void Awake()
        {
            m_font = GetComponent<TextMeshProUGUI>();
            m_text = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            m_font.font = YUR_Manager.Instance.YURSettings.Font;
            m_text.fontMaterial.renderQueue = YUR_Manager.Instance.YURSettings.WatchRenderQueue + 20;
        }
    }
}
