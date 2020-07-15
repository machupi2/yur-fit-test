using UnityEngine;

namespace YUR.SDK.Core.Utils
{
    /// <summary>
    /// This class is here due to certain bugs in IL2CPP builds not being able enable/disable colliders on UnityEvents
    /// </summary>
    public class ToggleCollider : MonoBehaviour
    {
        Collider coll;


        // Start is called before the first frame update
        void Start()
        {
            coll = GetComponent<Collider>();
        }

        // Update is called once per frame
        public void EnableCollider()
        {
            coll.enabled = true;
        }

        public void DisableCollider()
        {
            coll.enabled = false;
        }
    } 
}
