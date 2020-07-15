using UnityEngine;
using TMPro;
using YUR.Fit.Unity.Utilities;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Set PIN and URL text on PIN Sleeve
    /// </summary>
    public class PINTextUpdate : MonoBehaviour
    {
        [SerializeField] private TMP_Text _pinText = null, _urlText = null;

        public void SetTextSize(float size)
        {
            _pinText.fontSize = size;
            _urlText.fontSize = size;
        }

        public void UpdateText(YURShortcodeResponse response)
        {
            _pinText.text = response.ShortCode;
            _urlText.text = response.VerificationURL;
        }
    } 
}
