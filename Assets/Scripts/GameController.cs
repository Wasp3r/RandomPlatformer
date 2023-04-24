using System;
using RandomPlatformer.ScoringSystem;
using RandomPlatformer.UI;
using RandomPlatformer.UI.Menus;
using RandomPlatformer.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
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
        ///     Pause menu reference.
        /// </summary>
        [SerializeField] private PauseMenu _pauseMenu;

        /// <summary>
        ///    Score controller reference.
        /// </summary>
        [SerializeField] private ScoreController _scoreController;

        /// <summary>
        ///     Score controller getter.
        /// </summary>
        public ScoreController ScoreController => _scoreController;
        
        /// <summary>
        ///     Input actions getter.
        /// </summary>
        public DefaultInputActions InputActions => _inputActions;
        
        /// <summary>
        ///     Input actions.
        ///     It is used to listen to input events.
        /// </summary>
        private DefaultInputActions _inputActions;
        
        /// <summary>
        ///     Current game state.
        /// </summary>
        private GameState _gameState = GameState.Menu;
        
        /// <summary>
        ///     Listen to events.
        /// </summary>
        private void Awake()
        {
            Instance = this;
            _mainMenu.OnStartGame += StartNewGame;
            _inputActions = new DefaultInputActions();
            ListenToInput();
        }
        
        /// <summary>
        ///     Updates the current game state.
        /// </summary>
        /// <param name="gameState">New game state.</param>
        public void UpdateGameState(GameState gameState)
        {
            _gameState = gameState;
            switch (gameState)
            {
                case GameState.Active:
                    break;
                case GameState.Paused:
                    _pauseMenu.Enable();
                    break;
                case GameState.Menu:
                    _mainMenu.Enable();
                    break;
                case GameState.Exit:
                    ExitGame();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }
        }

        /// <summary>
        ///     Disables the main menu and starts the game.
        /// </summary>
        private void StartNewGame()
        {
            _mainMenu.Disable();
            _gameState = GameState.Active;
            SceneManager.LoadScene("Level_0", LoadSceneMode.Additive);
        }

        /// <summary>
        ///     Listen to input events.
        /// </summary>
        private void ListenToInput()
        {
            _inputActions.UI.Cancel.performed += OnCancel;
        }
        
        /// <summary>
        ///     On cancel input event.
        ///     TODO: Refactor this. Use a state machine instead.
        /// </summary>
        /// <param name="context">Input context.</param>
        private void OnCancel(InputAction.CallbackContext context)
        {
            switch (_gameState)
            {
                case GameState.Active:
                    _pauseMenu.Enable();
                    break;
                case GameState.Paused:
                    _pauseMenu.Disable();
                    _gameState = GameState.Active;
                    break;
                case GameState.Menu:
                    ExitGame();
                    break;
                case GameState.Leaderboard:
                    _mainMenu.Enable();
                    break;
            }
        }

        /// <summary>
        ///     Exits the game completely.
        /// </summary>
        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    public enum GameState
    {
        Active,
        Paused,
        Menu,
        Leaderboard,
        Exit
    }
}