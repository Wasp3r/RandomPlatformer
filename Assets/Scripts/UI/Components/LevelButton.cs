using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RandomPlatformer.UI.Components
{
    /// <summary>
    ///     Level button class used to display the level button.
    /// </summary>
    public class LevelButton : MonoBehaviour
    {
        /// <summary>
        ///     The button reference.
        /// </summary>
        [SerializeField] private Button _button;
        
        /// <summary>
        ///     The button text reference.
        /// </summary>
        [SerializeField] private TMP_Text _buttonText;
        
        private int _levelIndex;
        
        /// <summary>
        ///     Event called when the button is clicked.
        /// </summary>
        public event Action<int> OnLevelButtonClicked;

        /// <summary>
        ///     Listen to events.
        /// </summary>
        private void OnEnable()
        {
            _button.onClick.AddListener(TriggerButtonClicked);
        }
        
        /// <summary>
        ///     Stop listening to events.
        /// </summary>
        private void OnDisable()
        {
            _button.onClick.RemoveListener(TriggerButtonClicked);
        }

        /// <summary>
        ///     Remove all the listeners.
        /// </summary>
        private void OnDestroy()
        {
            OnLevelButtonClicked = null;
        }

        /// <summary>
        ///     Set the level name.
        /// </summary>
        /// <param name="levelIndex">The level index.</param>
        public void Initialize(int levelIndex)
        {
            _buttonText.text = $"Level {levelIndex + 1}";
            _levelIndex = levelIndex;
        }
        
        /// <summary>
        ///     Trigger the button clicked event.
        /// </summary>
        private void TriggerButtonClicked()
        {
            OnLevelButtonClicked?.Invoke(_levelIndex);
        }
    }
}