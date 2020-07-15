using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YUR.SDK.Core.Utils
{
    public class FormatTest : MonoBehaviour
    {
        public bool DebugRun = false;

        public float[] floatArray = {1.0f, 1.0f, 1.0f};

    private void Update()
        {
            if (DebugRun)
            {
                DebugRun = false;
                RunTest();
            }
        }

        void RunTest()
        {
            Debug.Log($"Convert as Vector3 with {YUR_Formatter.ConvertAs(floatArray, Enums.YurFormat.Vector3)}");
            Debug.Log($"Convert as ConvertToShortK");
            Debug.Log($"");
            Debug.Log($"");
            Debug.Log($"");
            Debug.Log($"");
            Debug.Log($"");
            Debug.Log($"");
            Debug.Log($"");
            Debug.Log($"");
        }
    } 
}
