using UnityEngine;
using YUR.SDK.Core.Enums;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Reverses GameObject Transforms based on hand alignment
    /// </summary>
    public class CheckScale : MonoBehaviour
    {
        [Header("HandScale")]
        public Vector3 Left = Vector3.one;
        public Vector3 Right = Vector3.one;

        private YURWatch m_watch = null;

        private void Awake()
        {
            m_watch = GameObject.FindObjectOfType<YURWatch>();
            m_watch.YURSettingsAsset.WatchHandChanged.AddListener(RunScaleCheck);
        }

        private void OnEnable()
        {
            RunScaleCheck();
        }

        private void RunScaleCheck()
        {
            Vector3 scaleFactor = Vector3.one;

            transform.localScale = new Vector3
                (
                    Mathf.Abs(transform.localScale.x),
                    Mathf.Abs(transform.localScale.y),
                    Mathf.Abs(transform.localScale.z)
                );

            switch (m_watch.YURSettingsAsset.HandBeingUsed)
            {
                case HandState.Left:
                    scaleFactor = Left;
                    break;
                case HandState.Right:
                    scaleFactor = Right;
                    break;
                default:
                    break;
            }

            transform.localScale = new Vector3
                (
                    transform.localScale.x * scaleFactor.x,
                    transform.localScale.y * scaleFactor.y,
                    transform.localScale.z * scaleFactor.z
                );
        }

        private void OnDestroy()
        {
            m_watch.YURSettingsAsset.WatchHandChanged.RemoveListener(RunScaleCheck);
        }
    }
}
