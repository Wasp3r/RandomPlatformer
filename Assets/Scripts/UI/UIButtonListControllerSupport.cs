using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RandomPlatformer.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RandomPlatformer.UI
{
    /// <summary>
    ///     This class is used to support the controller input in UI with button list.
    /// </summary>
    public class UIButtonListControllerSupport : MonoBehaviour
    {
        /// <summary>
        ///     The prefab of the controller indicator.
        /// </summary>
        [SerializeField] private GameObject _controllerIndicatorPrefab;

        private Transform _controllerIndicator;
        private Transform _parent;
        private bool _enabled;
        private List<GameObject> _buttons = new();
        private int _currentButtonIndex;

        /// <summary>
        ///     Subscribe to the controller connected and disconnected events.
        /// </summary>
        private void OnEnable()
        {
            if (!ControllerChecker.IsInitialized)
                return;
            
            ControllerChecker.Instance.OnControllerConnected += ShowIndicator;
            ControllerChecker.Instance.OnControllerDisconnected += HideIndicator;
        }

        /// <summary>
        ///     Unsubscribe from the controller connected and disconnected events.
        /// </summary>
        private void OnDisable()
        {
            if (!ControllerChecker.IsInitialized)
                return;
            
            ControllerChecker.Instance.OnControllerConnected -= ShowIndicator;
            ControllerChecker.Instance.OnControllerDisconnected -= HideIndicator;
        }

        /// <summary>
        ///     Map the buttons and subscribe to the controller input events.
        /// </summary>
        private void Start()
        {
            var inputActions = GameStateController.Instance.InputActions;
            _parent = transform.parent;
            MapButtons();

            if (!_buttons.Any())
            {
                Debug.LogWarning($"### - No buttons found in the list! Menu name {_parent.name}");
                return;
            }
            
            ControllerChecker.Instance.OnControllerConnected += ShowIndicator;
            ControllerChecker.Instance.OnControllerDisconnected += HideIndicator;
            inputActions.UI.Move.performed += MovePerformed;
            inputActions.UI.Submit.performed += SubmitPerformed;
            _currentButtonIndex = 0;
        }
        
        /// <summary>
        ///     Map the buttons in the list.
        /// </summary>
        private void MapButtons()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.GetComponent<Button>())
                    _buttons.Add(child.gameObject);
            }
        }

        /// <summary>
        ///     React to the controller submit input.
        /// </summary>
        /// <param name="input">The input context.</param>
        private void SubmitPerformed(InputAction.CallbackContext input)
        {
            if (!_enabled)
                return;
            
            _buttons[_currentButtonIndex].GetComponent<Button>().onClick.Invoke();
        }

        /// <summary>
        ///     React to the controller move input.
        /// </summary>
        /// <param name="input">The input context.</param>
        private void MovePerformed(InputAction.CallbackContext input)
        {
            if (!_enabled)
                return;
            
            var value = input.ReadValue<Vector2>();
            if (value.y == 0)
                return;
            
            if (value.y > 0)
                MoveUp();
            else
                MoveDown();
        }

        /// <summary>
        ///     Show the controller indicator.
        /// </summary>
        private void ShowIndicator()
        {
            if (!_controllerIndicator)
                _controllerIndicator = Instantiate(_controllerIndicatorPrefab, _parent).transform;
            
            _enabled = true;
            _controllerIndicator.gameObject.SetActive(true);
            _controllerIndicator.position = _buttons[_currentButtonIndex].transform.position;
        }

        /// <summary>
        ///     Hide the controller indicator.
        /// </summary>
        private void HideIndicator()
        {
            if (!_controllerIndicator)
                return;
            
            _enabled = false;
            _controllerIndicator.gameObject.SetActive(false);
        }

        /// <summary>
        ///     Move the indicator up.
        /// </summary>
        private void MoveUp()
        {
            if (!_enabled)
                return;
            
            if (_currentButtonIndex <= 0)
                _currentButtonIndex = _buttons.Count - 1;
            else
                _currentButtonIndex--;

            Debug.Log($"### - Move up! {_currentButtonIndex}");
        }

        /// <summary>
        ///     Move the indicator down.
        /// </summary>
        private void MoveDown()
        {
            if (!_enabled)
                return;
            
            if (_currentButtonIndex >= _buttons.Count - 1)
                _currentButtonIndex = 0;
            else
                _currentButtonIndex++;
            
            Debug.Log($"### - Move down! {_currentButtonIndex}");
        }
    }
}