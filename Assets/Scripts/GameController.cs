using System;
using RandomPlatformer.ScoringSystem;
using RandomPlatformer.UI;
using RandomPlatformer.UI.Menus;
using RandomPlatformer.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RandomPlatformer
{
    /// <summary>
    ///     The main game controller.
    ///     It is responsible for managing the game components like UI and game logic.
    /// </summary>
    public class GameController : MonoBehaviorSingleton<GameController>
    {
        /// <summary>
        ///     Main menu reference.
        /// </summary>
        [SerializeField] private MainMenu _mainMenu;

        /// <summary>
        ///    Score controller reference.
        /// </summary>
        [SerializeField] private ScoreController _scoreController;
        
        /// <summary>
        ///     Score controller getter.
        /// </summary>
        public ScoreController ScoreController => _scoreController;
        
        /// <summary>
        ///     Listen to events.
        /// </summary>
        private void Awake()
        {
            Instance = this;
            _mainMenu.OnStartGame += StartNewGame;
        }

        /// <summary>
        ///     Disables the main menu and starts the game.
        /// </summary>
        private void StartNewGame()
        {
            _mainMenu.Disable();
            SceneManager.LoadScene("Level_0");
        }
    }
}