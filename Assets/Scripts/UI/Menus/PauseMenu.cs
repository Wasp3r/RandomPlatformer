using System;
using RandomPlatformer.UI.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RandomPlatformer.UI.Menus
{
    /// <summary>
    ///     The pause menu.
    ///     This class is responsible for the pause menu UI.
    /// </summary>
    public class PauseMenu : UIFadable
    {
        /// <summary>
        ///     Resume button to resume the game.
        /// </summary>
        [SerializeField] private Button _resumeButton;

        /// <summary>
        ///     Menu button to go to the main menu.
        /// </summary>
        [SerializeField] private Button _goToMenuButton;

        /// <summary>
        ///     Exit button to exit the game.
        /// </summary>
        [SerializeField] private Button _exitButton;
        
        /// <summary>
        ///     Event to invoke when the resume button is clicked.
        /// </summary>
        public event Action OnResume;
        
        /// <summary>
        ///     Event to invoke when the menu button is clicked.
        /// </summary>
        public event Action OnGoToMenu;
        
        /// <summary>
        ///     Event to invoke when the exit button is clicked.
        /// </summary>
        public event Action OnExit;

        /// <summary>
        ///     Listen to events.
        /// </summary>
        private void OnEnable()
        {
            _resumeButton.onClick.AddListener(ResumeGame);
            _goToMenuButton.onClick.AddListener(GoToMenu);
            _exitButton.onClick.AddListener(ExitGame);
            
#if UNITY_WEBGL
            // Disable the exit button as it is not supported on WebGL.
            _exitButton.gameObject.SetActive(false);
#endif
        }

        /// <summary>
        ///     Stop listening to events.
        /// </summary>
        private void OnDisable()
        {
            _resumeButton.onClick.RemoveListener(ResumeGame);
            _goToMenuButton.onClick.RemoveListener(GoToMenu);
            _exitButton.onClick.RemoveListener(ExitGame);
        }

        /// <summary>
        ///     Resumes the game.
        /// </summary>
        private void ResumeGame()
        {
            OnResume?.Invoke();
        }

        /// <summary>
        ///     Go to main menu.
        /// </summary>
        private void GoToMenu()
        {
            OnGoToMenu?.Invoke();
        }
        
        /// <summary>
        ///     Exits the game.
        /// </summary>
        private void ExitGame()
        {
            OnExit?.Invoke();
        }
    }
}