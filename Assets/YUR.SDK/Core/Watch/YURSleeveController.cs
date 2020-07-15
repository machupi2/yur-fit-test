using UnityEngine;
using YUR.SDK.Core.Enums;
using YUR.Fit.Unity;
using YUR.SDK.Core.Initialization;
using System.Collections;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Loads widgets, adds them to the sleeve, and updates them via the YUR Service
    /// </summary>
    public class YURSleeveController : MonoBehaviour
    {
        [SerializeField] private SleeveState _currentSleeveState = SleeveState.None;
        public SleeveState CurrentSleeveState { get => _currentSleeveState; set => _currentSleeveState = value; }

        public SleeveState InitialSleeve = SleeveState.PINSleeve;

        [Header("Sleeve References")]        
        [SerializeField] private GameObject[] _sleeves = null;
        [SerializeField] private TriggerSleeveState[] _sleeveStateTriggers = null;

        [Header("Blending References")]
        [SerializeField] private SetBlend _metalBarsBlend = null;
        [SerializeField] private SetBlend _sleeveAnchorBlend = null;

        /// <summary>
        /// Private References
        /// </summary>
        private SleeveState m_previousSleeve = SleeveState.None;
        private Coroutine m_co = null;
        private bool m_wasLoggedIn = false;

        private void Awake()
        {
            foreach (TriggerSleeveState sleeveStateTrigger in _sleeveStateTriggers)
            {
                sleeveStateTrigger.referenceToSleeveController = this;
            }

            CoreServiceManager.OnLoginStateChanged += OnLoginStateChanged;
        }


        private void LateUpdate()
        {
            if (gameObject.activeInHierarchy && m_co is null)
                CheckSleeveState(m_previousSleeve, CurrentSleeveState);
        }

        private void OnDestroy()
        {
            CoreServiceManager.OnLoginStateChanged -= OnLoginStateChanged;
        }

        private void OnLoginStateChanged(bool loggedIn)
        {
            YUR_Manager.Instance.Log("Checking Login State");

            try
            {
                // Going from not logged in to logged in
                if (loggedIn && !m_wasLoggedIn)
                {
                    YUR_Manager.Instance.Log("Going to Setup Sleeve");
                    m_previousSleeve = SleeveState.SetupSleeve;
                    CurrentSleeveState = SleeveState.SetupSleeve;
                    TransitionToSleeve(SleeveState.SetupSleeve);
                    m_wasLoggedIn = true;
                }
                // Going from logged in to not logged in
                else if (!loggedIn && m_wasLoggedIn)
                {
                    YUR_Manager.Instance.Log($"Going to {InitialSleeve}");
                    m_previousSleeve = InitialSleeve;
                    CurrentSleeveState = InitialSleeve;
                    TransitionToSleeve(InitialSleeve);
                    m_wasLoggedIn = false;
                }
                // Initial state
                else if (!loggedIn && !m_wasLoggedIn)
                {
                    m_previousSleeve = InitialSleeve;
                    CurrentSleeveState = InitialSleeve;
                    TransitionToSleeve(InitialSleeve);
                }
            } catch(UnityException e)
            {
                YUR_Manager.Instance.Log("Login State Change event couldn't swap. Here's why: " + e.Message);
            }
        }

        private bool IsSleeveExpanded()
        {
            foreach (GameObject sleeve in _sleeves)
            {
                if (sleeve.GetComponent<TriggerContent>().isActive)
                    return true;
            }
            return false;
        }

        public void ToggleSleeveExpand()
        {
            if (IsSleeveExpanded())
            {
                StartCoroutine(RunSleeveRetract(SleeveState.None.ToString()));

            }
            else
            {
                StartCoroutine(RunSleeveExpand(m_previousSleeve.ToString()));
            }
        }

        #region SleeveCode
        /// Constantly check to see if the sleeve needs to change
        private void CheckSleeveState(SleeveState previousSleeve, SleeveState currentSleeve)
        {
            if (previousSleeve != currentSleeve)
            {
                YUR_Manager.Instance.Log("Transition Sleeve to " + currentSleeve + " from " + previousSleeve);
                TransitionToSleeve(currentSleeve);
                m_previousSleeve = currentSleeve;
            }
        }
        
        /// Transitions watch from one sleeve to the next.
        internal void TransitionToSleeve(SleeveState sleeveToTransitionTo)
        {
            if (gameObject.activeSelf)
                m_co = StartCoroutine(RunSleeveRetract(sleeveToTransitionTo.ToString()));
        }

        /// Retracts and turns off content on sleeve.
        private IEnumerator RunSleeveRetract(string sleeveName)
        {
            /// Tell the objects to start their blend to retract.
            while (_sleeveAnchorBlend.BlendValue >= 0)
            {
                _metalBarsBlend.BlendValue -= 0.025f;
                _sleeveAnchorBlend.BlendValue -= 0.03f;
                yield return null;
            }

            /// Will always turn off all sleeves as a precautionary measure.
            foreach (GameObject sleeve in _sleeves)
            {
                sleeve.GetComponent<TriggerContent>().Toggle(false);
            }

            /// Turns off mesh renderer if it is present
            foreach (GameObject sleeve in _sleeves)
            {
                if (sleeve.GetComponent<MeshRenderer>() != null)
                {
                    sleeve.GetComponent<MeshRenderer>().enabled = false;
                }
            }

            /// If we are going to a None Sleeve State, continue blending the Metal Bars to 0.
            /// If we are not, then extend the new sleeve to show.
            if (sleeveName != "None")
            {
                m_co = StartCoroutine(RunSleeveExpand(sleeveName));
            } else
            {
                m_co = null;

                /// Tell the objects to start their blend to retract.
                while (_metalBarsBlend.BlendValue >= 0.0f)
                {
                    _metalBarsBlend.BlendValue -= 0.025f;
                    yield return null;
                }
            }
        }


        private IEnumerator RunSleeveExpand(string sleeveName)
        {
            yield return null;

            GameObject thisSleeve = null;

            // Sets the correct sleeve
            foreach (GameObject sleeve in _sleeves)
            {
                if (sleeve.name == sleeveName)
                {
                    thisSleeve = sleeve;
                }
            }

            MeshRenderer rend = thisSleeve.GetComponent<MeshRenderer>();
            bool turnedOn = false;

            // Tell the objects to start their blend to retract.
            while (_metalBarsBlend.BlendValue < 1.0f)
            {
                _metalBarsBlend.BlendValue += 0.025f;

                if (_metalBarsBlend.BlendValue > 0.25f)
                {
                    if (!turnedOn)
                    {
                        // Turns on the sleeve
                        thisSleeve.GetComponent<TriggerContent>().Toggle(true);

                        if (rend != null)
                        {
                            rend.enabled = true;
                        }

                        turnedOn = true;
                    }

                    _sleeveAnchorBlend.BlendValue += 0.035f;
                }
                
                yield return null;
            }

            _metalBarsBlend.BlendValue = 1.0f;
            _sleeveAnchorBlend.BlendValue = 1.0f;

            m_co = null;
        }
        #endregion
    }
}