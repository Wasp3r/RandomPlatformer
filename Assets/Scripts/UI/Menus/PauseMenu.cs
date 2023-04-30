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
        ///     Input actions.
        ///     We need it for the controller/keyboard support.
        /// </summary>
        private DefaultInputActions _inputActions;
        
        /// <summary>
        ///     Get the input actions.
        /// </summary>
        private void Start()
        {
            _inputActions = GameStateController.Instance.InputActions;
        }

        /// <summary>
        ///     Listen to events.
        /// </summary>
        private void OnEnable()
        {
            _resumeButton.onClick.AddListener(ResumeGame);
            _goToMenuButton.onClick.AddListener(GoToMenu);
            _exitButton.onClick.AddListener(ExitGame);
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
        ///     Enables the pause menu.
        /// </summary>
        public override void Enable()
        {
            base.Enable();
            Time.timeScale = 0;
        }
        
        /// <summary>
        ///     Disables the pause menu.
        /// </summary>
        public override void Disable()
        {
            base.Disable();
            Time.timeScale = 1;
        }

        /// <summary>
        ///     Resumes the game.
        /// </summary>
        private void ResumeGame()
        {
            Disable();
            GameStateController.Instance.UpdateGameState(GameState.Active);
        }

        /// <summary>
        ///     Go to main menu.
        /// </summary>
        private void GoToMenu()
        {
            Disable();
            GameStateController.Instance.UpdateGameState(GameState.Menu);
        }
        
        /// <summary>
        ///     Exits the game.
        /// </summary>
        private void ExitGame()
        {
            Disable();
            GameStateController.Instance.UpdateGameState(GameState.Exit);
        }
    }
}