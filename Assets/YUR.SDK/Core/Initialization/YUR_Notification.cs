using UnityEngine;
using UnityEngine.Events;
using YUR.Fit.Core.Models;
using YUR.SDK.Core.Interfaces;
using YUR.SDK.Core.Utils;

namespace YUR.SDK.Core.Initialization
{
    public class YUR_Notification : MonoBehaviour, IYURUpdatable
    {
        public UnityEvent OnNotification = null;
        public UnityEventString OnNotificationHeader = null;
        public UnityEventString OnNotificationBody = null;

        [SerializeField] private string _debugHeaderString = "Hi";
        [SerializeField] private string _debugBodyString = "Hello There!";
        [SerializeField] private bool _debugFire = false;

        private void OnEnable()
        {
            YUR_Manager.Instance.OnYURUpdate.AddListener(ApplyOverlayUpdate);
        }

        private void OnDisable()
        {
            YUR_Manager.Instance.OnYURUpdate.RemoveListener(ApplyOverlayUpdate);
        }

        private void Update()
        {
            if (_debugFire)
            {
                _debugFire = false;

                if (!string.IsNullOrEmpty(_debugHeaderString) && !string.IsNullOrEmpty(_debugBodyString))
                {
                    OnNotificationHeader.Invoke(_debugHeaderString);
                    OnNotificationBody.Invoke(_debugBodyString);
                    OnNotification.Invoke();
                }
            }
        }

        public void ApplyOverlayUpdate(OverlayStatusUpdate OSU)
        {
            string header = OSU.CurrentNotification.HeaderText;
            string body = OSU.CurrentNotification.Body;

            if (!string.IsNullOrEmpty(header) && !string.IsNullOrEmpty(body))
            {
                YUR_Manager.Instance.Log($"Notification Found! Here's the header: {header}");
                YUR_Manager.Instance.Log($"Notification Found! Here's the body: {body}");
                OnNotificationHeader.Invoke(header);
                OnNotificationBody.Invoke(body);
                OnNotification.Invoke();
            }
        }
    } 
}
