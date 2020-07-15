using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;
using UnityEngine.UI;
using YUR.SDK.Core.Interaction;
using YUR.SDK.Core.Initialization;

/// <summary>
/// Uses a raycast to place a dot on to the watch 
/// and fires events on the watch where applicable.
/// </summary>
public class Dot : MonoBehaviour
{
    public GameObject Guide;
    public Vector3 RaycastOffset = Vector3.zero;
    public float FingerSeconds = 1;
    public float UpFactor = 4.0F;
    public float OpacityInTime = 0.1F;
    public float OpacityOutTime = .5F;
    public bool IsDown = false;
    public float _currentPos = 0;
    public Image FillCircle;
    internal LayerMask LayerToHit;

    // Set opacity to 1 at start
    public float _initialOpacity = 1;
    private float _opacity = 0;
    private Collider _lastCollider;
    private List<XRNodeState> _nodeStates = new List<XRNodeState>();

    private void Start()
    {
        LayerToHit |= (1 << LayerMask.NameToLayer(YUR_Manager.Instance.YURSettings.LayerToSet));
    }

    private void Update()
    {
        // Get the center eye position and rotation
        InputTracking.GetNodeStates(_nodeStates);
        var centerEye = _nodeStates.FirstOrDefault(x => x.nodeType == XRNode.CenterEye);
        Vector3 pos;
        Quaternion rot;

        bool isHit = false;

        // Try to get pos and rot of center eye
        if (centerEye.TryGetPosition(out pos) && centerEye.TryGetRotation(out rot))
        {
            RaycastHit hit;

            // Check Raycast for hittable object
            if (Physics.Raycast(pos + RaycastOffset , rot * Vector3.forward, out hit, 2.0f, LayerToHit))
            {
                // If hitting the same collider, increment the fill wheel
                if (hit.collider == _lastCollider)
                {
                    Guide.SetActive(true);
                    GazeEvent gaze = hit.collider.gameObject.GetComponent<GazeEvent>();

                    if (gaze != null)
                    {
                        _currentPos += Time.unscaledDeltaTime / FingerSeconds;

                        if (_currentPos > 1)
                        {
                            _currentPos = 1;

                            // Fire!
                            if (!IsDown)
                            {
                                IsDown = true;
                                if (gaze != null)
                                {
                                    gaze.FireGazeEvent();
                                }
                            }
                        }
                    } else
                    {

                    }
                }
                else
                {
                    // Reset gaze interaction
                    IsDown = false;
                    _currentPos = 0;
                    _lastCollider = hit.collider;
                }

                // Fill wheel follows gaze
                transform.position = hit.point;

                isHit = true;
            }
        }

        // If nothing was hit, reduce wheel fill
        if (!isHit)
        {
            Guide.SetActive(false);
            _currentPos -= (Time.unscaledDeltaTime / FingerSeconds) * UpFactor;
            if (_currentPos < 0)
            {
                _currentPos = 0;
            }
            if (_currentPos == 0 && IsDown)
            {
                IsDown = false;
            }
        }

        // If something was hit, and isn't down
        if (isHit && !IsDown)
        {
            if (_opacity < 1)
            {
                _opacity += (Time.unscaledDeltaTime / OpacityInTime);
                if (_opacity > 1)
                {
                    _opacity = 1;
                }
            }
            FillCircle.fillAmount = _currentPos;

        } else if (!isHit || IsDown)
        {
            if (IsDown)
            {
                _opacity -= (Time.unscaledDeltaTime / OpacityInTime);

            }
            else
            {
                _opacity -= (Time.unscaledDeltaTime / OpacityOutTime);
            }
            if (_opacity <= 0)
            {
                _opacity = 0;

                if (!IsDown)
                {
                    _lastCollider = null;
                }
            }

            if (!IsDown)
            {
                _lastCollider = null;
            } 

        }
        if (IsDown)
        {
            FillCircle.fillAmount = 0;
        }
        else
        {
            FillCircle.fillAmount = _currentPos;
        }
    }
}
