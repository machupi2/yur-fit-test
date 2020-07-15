using System.Collections.Generic;
using UnityEngine;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Stores an array of light indicators to change color on
    /// </summary>
    public class ChangeIndicatorMaterials : MonoBehaviour
    {
        public Material onlineMat;
        public Material offlineMat;

        private List<Renderer> m_indicatorRenderers = new List<Renderer>();

        private void OnEnable()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                m_indicatorRenderers.Add(transform.GetChild(i).GetComponent<Renderer>());
            }
        }

        public void GoOnline()
        {
            foreach(Renderer rend in m_indicatorRenderers)
            {
                rend.material = onlineMat;
            }
        }

        public void GoOffline()
        {
            foreach (Renderer rend in m_indicatorRenderers)
            {
                rend.material = offlineMat;
            }
        }
    } 
}
