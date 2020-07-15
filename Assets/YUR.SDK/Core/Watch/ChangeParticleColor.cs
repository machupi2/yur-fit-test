using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YUR.SDK.Core.Watch
{
    /// <summary>
    /// Changes the particle color on the Legendary watch
    /// </summary>
    public class ChangeParticleColor : MonoBehaviour
    {
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            _particleSystem.Play();
        }

        public void GoOnline()
        {
            if (_particleSystem is object)
            {
                var main = _particleSystem.main;
                main.startColor = Color.green;
            }
        }

        public void GoOffline()
        {
            if (_particleSystem is object)
            {
                var main = _particleSystem.main;
                main.startColor = Color.red;
            }
        }
    } 
}
