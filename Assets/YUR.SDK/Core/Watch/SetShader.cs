using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YUR.SDK.Core.Configuration;
using YUR.SDK.Core.Initialization;

namespace YUR.SDK.Core.Watch
{
	public enum ShaderOverrideType
	{
		WatchAndTile,
		TMP,
		Image
	}

	public class SetShader : MonoBehaviour
	{
		public ShaderOverrideType shaderType = ShaderOverrideType.WatchAndTile;

		private Renderer m_rend = null;
		private TMP_Text m_text = null;
		private Image m_image = null;

		private void Awake()
		{
			switch (shaderType)
			{
				case ShaderOverrideType.WatchAndTile:
					m_rend = GetComponent<Renderer>();
					break;
				case ShaderOverrideType.TMP:
					m_text = GetComponent<TMP_Text>();
					break;
				case ShaderOverrideType.Image:
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
				YUR_Settings settings = YUR_Manager.Instance.YURSettings;

				switch (shaderType)
				{
					case ShaderOverrideType.WatchAndTile:
						if (settings.WatchAndTileShaderOverride != null)
							m_rend.material.shader = settings.WatchAndTileShaderOverride;
						break;
					case ShaderOverrideType.TMP:
						if (settings.TmpShaderOverride != null)
							m_text.fontMaterial.shader = settings.TmpShaderOverride;
						break;
					case ShaderOverrideType.Image:
						if (settings.ImageShaderOverride != null)
							m_image.material.shader = settings.ImageShaderOverride;
						break;
					default:
						break;
				}
			} catch (Exception e)
			{
				YUR_Manager.Instance.Log($"Could not set shader because {e.Message}");
			}
		}
	}
}
