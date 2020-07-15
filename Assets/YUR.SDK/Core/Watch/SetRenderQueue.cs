using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YUR.SDK.Core.Initialization;

namespace YUR.SDK.Core.Watch
{
	public enum RenderQueueType
	{
		Tile,
		Text,
		Image
	}

	public class SetRenderQueue : MonoBehaviour
	{
		public RenderQueueType queueType = RenderQueueType.Tile;

		private Renderer m_rend = null;
		private TMP_Text m_text = null;
		private Image m_image = null;

		private void Awake()
		{
			switch (queueType)
			{
				case RenderQueueType.Tile:
					m_rend = GetComponent<Renderer>();
					break;
				case RenderQueueType.Text:
					m_text = GetComponent<TMP_Text>();
					break;
				case RenderQueueType.Image:
					m_image = GetComponent<Image>();
					break;
				default:
					break;
			}
		}

		private void OnEnable()
		{
			try
			{
				int queue = YUR_Manager.Instance.YURSettings.WatchRenderQueue;

				switch (queueType)
				{
					case RenderQueueType.Tile:
						m_rend.material.renderQueue = queue;
						break;
					case RenderQueueType.Text:
						m_text.fontMaterial.renderQueue = queue + 20;
						break;
					case RenderQueueType.Image:
						m_image.material.renderQueue = queue + 10;
						break;
					default:
						break;
				}
			} catch (Exception e)
			{
				YUR_Manager.Instance.Log(e);
			}
		}
	} 
}
