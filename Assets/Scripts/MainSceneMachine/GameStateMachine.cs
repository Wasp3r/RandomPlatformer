using System;
using System.Collections.Generic;
using RandomPlatformer.LevelSystem;
using RandomPlatformer.MainSceneMachine.States;
using RandomPlatformer.Player;
using RandomPlatformer.ScoringSystem;
using RandomPlatformer.UI.Menus;
using RandomPlatformer.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace RandomPlatformer.MainSceneMachine
{
    /// <summary>
    ///     Main game state machine.
    /// </summary>
    public class GameStateMachine : MonoBehaviorSingleton<GameStateMachine>
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
        ///     Current state;
        /// </summary>
        private BaseState _currentState;
        
        /// <summary>
        ///     States dictionary
        /// </summary>
        private readonly Dictionary<State, BaseState> _statesDictionary = new();
        
        /// <summary>
        ///     Input actions.
        ///     It is used to listen to input events.
        /// </summary>
        private DefaultInputActions _inputActions;

        /// <summary>
        ///     Initialize the state machine.
        /// </summary>
        private void Awake()
        {
            InitializeStates();
            _inputActions = new DefaultInputActions();
            _inputModule.cancel.action.performed += OnCancel;
            
            GoToState(State.MainMenu);
        }

        /// <summary>
        ///     On cancel input is received.
        /// </summary>
        /// <param name="inputContext">Input context</param>
        private void OnCancel(InputAction.CallbackContext inputContext)
        {
            _currentState.OnCancel();
        }

        public void GoToState(State stateToGoTo)
        {
            if (!_statesDictionary.ContainsKey(stateToGoTo))
            {
                Debug.LogError($"### - State {stateToGoTo} not present in the dictionary!");
                return;
            }

            var targetState = _statesDictionary[stateToGoTo];
            targetState.OnEnterState();
            if (_currentState == null)
            {
                Debug.Log("### - Current state is null. It should only happen on the beginning of the game.");
                return;
            }
            
            _currentState.OnExitState();
            _currentState = targetState;
        }

        /// <summary>
        ///     Initialize all the states.
        /// </summary>
        private void InitializeStates()
        {
            _statesDictionary.Add(State.MainMenu, new MainMenuState(_mainMenu));

            foreach (var state in _statesDictionary)
            {
                state.Value.Initialize(this);
            }
        }
    }
}