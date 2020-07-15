using UnityEngine;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Used to set the blend tree parameters on watch objects
    /// </summary>
    public class SetBlend : MonoBehaviour
    {
        public float BlendValue { get => _blendValue; set => _blendValue = value; }
        [SerializeField][Range(0,1)] private float _blendValue = 0.0f;

        private Animator m_thisAnimator = null;

        private void Awake()
        {
            m_thisAnimator = GetComponent<Animator>();
            m_thisAnimator.SetFloat("Blend", 0);
        }

        private void Update()
        {
            m_thisAnimator.SetFloat("Blend", _blendValue);
        }
    } 
}
