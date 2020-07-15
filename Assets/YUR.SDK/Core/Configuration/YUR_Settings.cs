using TMPro;
using UnityEngine;
using UnityEngine.Events;
using YUR.SDK.Core.Enums;

namespace YUR.SDK.Core.Configuration
{
    /// <summary>
    /// Script that allows for the generation of a YURSettings ScriptableObject, 
    /// which is used for configuring the watch
    /// </summary>
    [CreateAssetMenu]
    public class YUR_Settings : ScriptableObject
    {
        [Header("Transform Options")]
        [HideInInspector] public UnityEvent WatchHandChanged;

        private HandState _lastHand;
        public HandState HandBeingUsed
        {
            get
            {
                return _handBeingUsed;
            }
            set
            {
                bool changed = (value != _lastHand);
                _handBeingUsed = value;
                _lastHand = value;
                if (changed)
                {
                    WatchHandChanged?.Invoke();
                }
            }
        }
        [SerializeField]
        private HandState _handBeingUsed = HandState.Left;

        private void OnValidate()
        {
            if (HandBeingUsed != _lastHand)
            {
                _lastHand = HandBeingUsed;
                WatchHandChanged?.Invoke();
            }
        }
        
        [Header("Left Configuration")]
        [SerializeField]
        private Vector3 _leftPositionOffset = new Vector3(-0.0708f, -0.0201f, -0.1337f);
        public Vector3 LeftPositionOffset
        {
            get
            {
                return _leftPositionOffset;
            }
            set
            {
                _leftPositionOffset = value;
            }
        }
        [SerializeField]
        private Vector3 _leftEulerOffset = new Vector3(21.755f, 13.447f, 90.42001f);
        public Vector3 LeftEulerOffset
        {
            get
            {
                return _leftEulerOffset;
            }
            set
            {
                _leftEulerOffset = value;
            }
        }
        
        [Header("Right Configuration")]
        [SerializeField]
        private Vector3 _rightPositionOffset = new Vector3(-0.0708f, -0.0201f, -0.1337f);
        public Vector3 RightPositionOffset
        {
            get
            {
                return _rightPositionOffset;
            }
            set
            {
                _rightPositionOffset = value;
            }
        }
        [SerializeField]
        private Vector3 _rightEulerOffset = new Vector3(21.755f, 13.447f, 90.42001f);
        public Vector3 RightEulerOffset
        {
            get
            {
                return _rightEulerOffset;
            }
            set
            {
                _rightEulerOffset = value;
            }
        }

        [Header("Render Settings")]
        [SerializeField] private Shader _watchAndTileShaderOverride = null;
        public Shader WatchAndTileShaderOverride
        {
            get
            {
                return _watchAndTileShaderOverride;
            }
            set
            {
                _watchAndTileShaderOverride = value;
            }
        }

        [SerializeField] private Shader _tmpShaderOverride = null;
        public Shader TmpShaderOverride
        {
            get
            {
                return _tmpShaderOverride;
            }
            set
            {
                _tmpShaderOverride = value;
            }
        }

        [SerializeField] private Shader _imageShaderOverride = null;
        public Shader ImageShaderOverride
        {
            get
            {
                return _imageShaderOverride;
            }
            set
            {
                _imageShaderOverride = value;
            }
        }

        [SerializeField] private string _shaderColorProperty = string.Empty;
        public string ShaderColorProperty
        {
            get
            {
                return _shaderColorProperty;
            }
            set
            {
                _shaderColorProperty = value;
            }
        }

        [SerializeField] private int _watchRenderQueue = 3050;
        public int WatchRenderQueue
        {
            get
            {
                return _watchRenderQueue;
            }
            set
            {
                _watchRenderQueue = value;
            }
        }

        [Header("Layer Settings")]
        public string LayerToSet = "Default";

        [Header("Font Settings")]
        public TMP_FontAsset Font = null;

        [Header("YUR Service Settings")]
        [SerializeField] private bool _turnOffYURServiceOnDisable = false;
        public bool TurnOffYURServiceOnDisable 
        {
            get => _turnOffYURServiceOnDisable; 
            set => _turnOffYURServiceOnDisable = value; 
        }
    } 
}
