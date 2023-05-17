using System;
using System.Collections;
using UnityEngine;

namespace RandomPlatformer.Utils
{
    public class ControllerChecker : MonoBehaviorSingleton<ControllerChecker>
    {
        [SerializeField] private float _checkingInterval = 1f;
        
        public Action OnControllerConnected;
        public Action OnControllerDisconnected;
        
        private bool _controllerConnected;
        
        /// <summary>
        ///     Start a coroutine to constantly check if the controller is connected.
        /// </summary>
        private void Awake()
        {
            StartCoroutine(CheckControllerCoroutine());
        }
        
        /// <summary>
        ///     Check if the controller is connected.
        /// </summary>
        private IEnumerator CheckControllerCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_checkingInterval);
                var controllers = Input.GetJoystickNames();
                
                if (controllers.Length > 0)
                {
                    if (_controllerConnected) 
                        continue;

                    Debug.Log("### - Controller connected!");
                    _controllerConnected = true;
                    OnControllerConnected?.Invoke();
                }
                else
                {
                    if (!_controllerConnected) 
                        continue;

                    Debug.Log("### - Controller disconnected!");
                    _controllerConnected = false;
                    OnControllerDisconnected?.Invoke();
                }
            }
        }
    }
}