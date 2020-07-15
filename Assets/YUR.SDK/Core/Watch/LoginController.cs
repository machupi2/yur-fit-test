using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using YUR.Fit.Unity;
using YUR.Fit.Unity.Utilities;
using YUR.SDK.Core.Initialization;
using YUR.SDK.Core.Utils;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Controls how the PIN Sleeve logs in
    /// </summary>
    public class LoginController : MonoBehaviour
    {
        public ShortcodeLoginEvent OnLoginRequest = null; // Sends over shortcode (i.e. 725122)
        public UnityEvent OnLoginError = null;
        public UnityEvent OnLoginSuccess = null;

        private const float LOGIN_ATTEMPT_TIMEOUT = 10;
        private Coroutine _co = null;

        private void OnEnable()
        {
            // Set login events
            YUR_Manager.Instance.OnLoginAttempt.AddListener(SetShortcodeMessage);
            YUR_Manager.Instance.OnLoginFailed.AddListener(SetErrorcodeMessage);
            YUR_Manager.Instance.OnLoginSuccess.AddListener(SetSuccessMessage);

            // Will request new shortcode if necessary
            if (CoreServiceManager.LoginCancellationToken is null || CoreServiceManager.LoginCancellationToken.IsCancellationRequested)
            {
                Debug.Log("Requesting shortcode");
                YUR_Manager.Instance.Login();
                _co = StartCoroutine(LoginAttemptTimeout());
            }
            else
            {
                Debug.Log("Shortcode is still valid");
            }
        }

        private void SetShortcodeMessage(YURShortcodeResponse response)
        {
            OnLoginRequest.Invoke(response);
            StopCoroutine();
        }

        private void SetErrorcodeMessage()
        {
            OnLoginError.Invoke();
            StopCoroutine();
        }

        private void SetSuccessMessage()
        {
            OnLoginSuccess.Invoke();
            StopCoroutine();
        }

        private void StopCoroutine()
        {
            if (_co != null) 
            {
                StopCoroutine(_co);
                _co = null;
            }
        }

        public void RetryLogin()
        {
            StopCoroutine();
            YUR_Manager.Instance.Login();
        }

        private void OnDisable()
        {
            YUR_Manager.Instance.OnLoginAttempt.RemoveListener(SetShortcodeMessage);
            YUR_Manager.Instance.OnLoginFailed.RemoveListener(SetErrorcodeMessage);
            YUR_Manager.Instance.OnLoginSuccess.RemoveListener(SetSuccessMessage);
        }

        // If the login attempt doesn't fire in a reasonable time or we are on the error screen,
        // then just go right to the Start Screen
        private IEnumerator LoginAttemptTimeout()
        {
            WaitForSecondsRealtime wait = new WaitForSecondsRealtime(LOGIN_ATTEMPT_TIMEOUT);

            yield return wait;

            SetErrorcodeMessage();
        }
    } 
}
