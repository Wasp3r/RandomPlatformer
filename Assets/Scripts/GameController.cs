using System;
using RandomPlatformer.ScoringSystem;
using RandomPlatformer.UI;
using RandomPlatformer.UI.Menus;
using RandomPlatformer.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using GUI = UnityEngine.GUI;

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
        ///     Game over menu reference.
        /// </summary>
        [SerializeField] private LeaderBoard _leaderBoard;

        /// <summary>
        ///    Score controller reference.
        /// </summary>
        [SerializeField] private ScoreController _scoreController;

        /// <summary>
        ///     Camera controller reference.
        /// </summary>
        [SerializeField] private CameraController _cameraController;

        /// <summary>
        ///     GUI controller reference.
        /// </summary>
        [SerializeField] private GUIController _guiController;

        [SerializeField] private InputSystemUIInputModule _inputModule;

        /// <summary>
        ///     Score controller getter.
        /// </summary>
        public ScoreController ScoreController => _scoreController;
        
        /// <summary>
        ///     Input actions getter.
        /// </summary>
        public DefaultInputActions InputActions => _inputActions;
        
        /// <summary>
        ///     Camera controller getter.
        /// </summary>
        public CameraController CameraController => _cameraController;
        
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
            Debug.Log($"### - Changing state {_gameState} => {gameState}");
            if (gameState == _gameState)
                return;
            
            switch (gameState)
            {
                case GameState.Active:
                    _guiController.Enable();
                    break;
                case GameState.Paused:
                    _pauseMenu.Enable();
                    break;
                case GameState.Menu:
                    StopGame();
                    _guiController.Disable();
                    _mainMenu.Enable();
                    break;
                case GameState.Exit:
                    ExitGame();
                    break;
                case GameState.Leaderboard:
                    _mainMenu.Disable();
                    _leaderBoard.Enable();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }

            Debug.Log("### - State changed");
            _gameState = gameState;
        }

        /// <summary>
        ///     Disables the main menu and starts the game.
        /// </summary>
        private void StartNewGame()
        {
            _mainMenu.Disable();
            UpdateGameState(GameState.Active);
            SceneManager.LoadScene("Level_0", LoadSceneMode.Additive);
        }

        /// <summary>
        ///     Stops the game.
        ///     We need it to unload the game scene.
        /// </summary>
        private void StopGame()
        {
            if (SceneManager.sceneCount == 1)
                return;
            
            _cameraController.StopFollowing();
            SceneManager.UnloadSceneAsync("Level_0");
        }

        /// <summary>
        ///     Listen to input events.
        /// </summary>
        private void ListenToInput()
        {
            _inputModule.cancel.action.performed += OnCancel;
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
                    UpdateGameState(GameState.Paused);
                    break;
                case GameState.Paused:
                    _pauseMenu.Disable();
                    UpdateGameState(GameState.Active);
                    break;
                case GameState.Menu:
                    ExitGame();
                    break;
                case GameState.Leaderboard:
                    _leaderBoard.Disable();
                    UpdateGameState(GameState.Menu);
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