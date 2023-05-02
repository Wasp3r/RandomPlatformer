using System;
using RandomPlatformer.LevelSystem;
using RandomPlatformer.Player;
using RandomPlatformer.ScoringSystem;
using RandomPlatformer.UI;
using RandomPlatformer.UI.Menus;
using RandomPlatformer.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using GUI = UnityEngine.GUI;

namespace RandomPlatformer
{
    /// <summary>
    ///     The main game controller.
    ///     It is responsible for managing the game components like UI and game logic.
    /// </summary>
    public class GameStateController : MonoBehaviorSingleton<GameStateController>
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
        ///     Choose level menu reference.
        /// </summary>
        [SerializeField] private ChooseLevelMenu _chooseLevelMenu;

        /// <summary>
        ///     Game result controller reference.
        /// </summary>
        [SerializeField] private GameResultUIController gameResultUIController;

        /// <summary>
        ///    Score controller reference.
        /// </summary>
        [SerializeField] private ScoreController _scoreController;

        /// <summary>
        ///     Camera controller reference.
        /// </summary>
        [SerializeField] private CameraController _cameraController;

        /// <summary>
        ///     Lives controller reference.
        /// </summary>
        [SerializeField] private LivesController _livesController;

        /// <summary>
        ///     GUI controller reference.
        /// </summary>
        [SerializeField] private GUIController _guiController;

        /// <summary>
        ///     Level controller reference.
        /// </summary>
        [SerializeField] private LevelController _levelController;

        /// <summary>
        ///     Input module reference.
        /// </summary>
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
        ///     Level controller getter.
        /// </summary>
        public LevelController LevelController => _levelController;
        
        /// <summary>
        ///     Lives controller getter.
        /// </summary>
        public LivesController LivesController => _livesController;
        
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
            _livesController.OnAllLivesLost += GameLost;
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
                    _mainMenu.Enable();
                    break;
                case GameState.Exit:
                    ExitGame();
                    break;
                case GameState.Leaderboard:
                    _mainMenu.Disable();
                    _leaderBoard.Enable();
                    break;
                case GameState.ChooseLevel:
                    _mainMenu.Disable();
                    _chooseLevelMenu.Enable();
                    break;
                case GameState.Result:
                    StopGame();
                    gameResultUIController.ShowResult(false);
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
            _livesController.ResetLives();
            _levelController.OpenLevel(0);
        }

        /// <summary>
        ///     Stops the game.
        ///     We need it to unload the game scene.
        /// </summary>
        private void StopGame()
        {
            if (SceneManager.sceneCount == 1)
                return;
            
            _guiController.Disable();
            _cameraController.StopFollowing();
            _levelController.UnloadedCurrentLevel();
        }

        /// <summary>
        ///     Player lost the game.
        /// </summary>
        private void GameLost()
        {
            
            UpdateGameState(GameState.Result);
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
                case GameState.Result:
                    gameResultUIController.Disable();
                    UpdateGameState(GameState.Menu);
                    break;
                case GameState.ChooseLevel:
                    _chooseLevelMenu.Disable();
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
        ChooseLevel,
        Result,
        Exit
    }
}