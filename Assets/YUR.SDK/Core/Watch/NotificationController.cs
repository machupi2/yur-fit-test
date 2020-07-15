using System.Collections;
using UnityEngine;

namespace YUR.SDK.Core.Watch
{
    [RequireComponent(typeof(AudioSource))]
    public class NotificationController : MonoBehaviour
    {
        public GameObject NotificationPanel = null;
        public Transform RetractedTransform = null, ExpandedTransform = null;
        public AudioClip NotificationSound = null;
        public AnimationCurve NotificationCurve = null;
        public float TimeToMove = 1.0f;
        public float WaitTime = 6.0f;

        private AudioSource m_source = null;

        private void Awake()
        {
            m_source = GetComponent<AudioSource>();
        }

        public void Notify()
        {
            StartCoroutine(ExpandNotification());
        }

        private IEnumerator ExpandNotification()
        {
            m_source.PlayOneShot(NotificationSound);

            var currentPos = NotificationPanel.transform.localPosition;
            var currentScale = NotificationPanel.transform.localScale;

            var t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / TimeToMove;
                NotificationPanel.transform.localPosition = Vector3.Lerp(currentPos, ExpandedTransform.localPosition, NotificationCurve.Evaluate(t));
                NotificationPanel.transform.localScale = Vector3.Lerp(currentScale, ExpandedTransform.localScale, NotificationCurve.Evaluate(t));
                yield return null;
            }

            yield return new WaitForSeconds(WaitTime);

            StartCoroutine(RetractNotification());
        }

        private IEnumerator RetractNotification()
        {
            var currentPos = NotificationPanel.transform.localPosition;
            var currentScale = NotificationPanel.transform.localScale;

            var t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / TimeToMove;
                NotificationPanel.transform.localPosition = Vector3.Lerp(currentPos, RetractedTransform.localPosition, NotificationCurve.Evaluate(t));
                NotificationPanel.transform.localScale = Vector3.Lerp(currentScale, RetractedTransform.localScale, NotificationCurve.Evaluate(t));
                yield return null;
            }
        }
    } 
}
