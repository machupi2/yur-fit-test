using UnityEngine;
using YUR.Fit.Core.Models;
using YUR.SDK.Core.Initialization;
using YUR.SDK.Core.Interfaces;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// The base class used for setting arbitrary data that is being retrieved from the OverlayStatusUpdate
    /// </summary>
    public abstract class DataSetter : MonoBehaviour, IYURUpdatable
    {
        private string PropertyPathBase
        {
            get =>
                $"{gameObject.transform.parent.name}.{gameObject.name}";
        }

        public string PropertyPath
        {
            get => string.IsNullOrWhiteSpace(DataTag)?$"{PropertyPathBase}.{DefaultDataTag}": DataTag;
        }

        protected abstract string DefaultDataTag { get; }

        public string DataTag = string.Empty;

        [Header("Does Nothing if not Applicable")]
        public ObjectToTarget TargetedObject = ObjectToTarget.GameObject;

        public virtual void Awake()
        {
            gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
        }

        public virtual void OnEnable()
        {
            YUR_Manager.Instance.OnYURUpdate.AddListener(ApplyOverlayUpdate);
        }

        public virtual void OnDisable()
        {
            YUR_Manager.Instance.OnYURUpdate.RemoveListener(ApplyOverlayUpdate);
        }

        public virtual void ApplyOverlayUpdate(OverlayStatusUpdate OSU)
        {

        }
    } 
}

/*
 * Example of Arbitrary Data JSON
 * 
   "YUR_HeartRate": {
        "somearray": [
            "one",
            "two",
            "three"
        ],
        "someobject": {
            "two": "two",
            "one": 1,
            "three": [
                "a",
                "b",
                "c"
            ]
        },
        "somestring": "hiya",
        "somenumber": 1,
*/