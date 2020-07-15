using UnityEngine;
using UnityEngine.Events;
using YUR.Fit.Unity;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Acts as controller for everything that affects the watch prefab itself
    /// </summary>
    public class WatchReferenceContainer : MonoBehaviour
    {
        public GameObject ThisWatch { get; private set; }
        public UnityEvent GoOnlineEvent = null, GoOfflineEvent = null;
        public Renderer[] watchRenders = null;
        
        private void Awake()
        {
            ThisWatch = this.gameObject;
        }

        private void OnEnable()
        {
            CoreServiceManager.OnLoginStateChanged += SetLoginMat;
            
            // Run in case the watch goes offline/online while inactive
            SetLoginMat(CoreServiceManager.IsLoggedIn);
        }

        private void SetLoginMat(bool obj)
        {
            if (obj == true)
            {
                GoOnlineEvent.Invoke();
            } else
            {
                GoOfflineEvent.Invoke();
            }
        }

        private void OnDisable()
        {
            CoreServiceManager.OnLoginStateChanged -= SetLoginMat;
        }

        public void SetMaterial(Shader shaderToSet)
        {
            foreach (var rend in watchRenders)
            {
                rend.material.shader = shaderToSet;
            }
        }

        public void SetRenderQueue(int renderQueue)
        {
            foreach (var rend in watchRenders)
            {
                rend.material.renderQueue = renderQueue;
            }
        }
    }
}
