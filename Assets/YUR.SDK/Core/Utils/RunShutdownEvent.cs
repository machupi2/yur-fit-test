using UnityEngine;
using UnityEngine.Events;
using YUR.Fit.Unity;

namespace YUR.SDK.Core.Utils
{
	public class RunShutdownEvent : MonoBehaviour
	{
		public UnityEvent OnShutdown = null;

		private void OnEnable()
		{
			OnShutdown.Invoke();
			CoreServiceManager.Logout();
		}
	}
}
