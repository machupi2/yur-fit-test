using UnityEngine;
using YUR.SDK.Core.Enums;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Used for testing the hand swapping of the YUR.Watch
    /// </summary>
    public class TriggerHandChange : MonoBehaviour
    {
        private YURWatch m_watch = null;

        private void Awake()
        {
            m_watch = GameObject.FindObjectOfType<YURWatch>();
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (m_watch.YURSettingsAsset.HandBeingUsed)
            {
                case HandState.Left:
                    m_watch.YURSettingsAsset.HandBeingUsed = HandState.Right;
                    break;
                case HandState.Right:
                    m_watch.YURSettingsAsset.HandBeingUsed = HandState.Left;
                    break;
                default:
                    break;
            }
        }
    }

}