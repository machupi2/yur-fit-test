using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YUR.SDK.Core.Utils
{
	public class RunRetryEvent : MonoBehaviour
	{
		public UnityEvent OnRetry = null;

		[SerializeField] private bool _retryOnEnable = false;

		private void OnEnable()
		{
			if (_retryOnEnable)
			{
				OnRetry.Invoke();
			}
		}

		public void DoRetry()
		{
			OnRetry.Invoke();
		}
	} 
}
