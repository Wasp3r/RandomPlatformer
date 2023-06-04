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
        [SerializeField] private GameResultUIController _gameResultUIController;

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
        ///     Time controller reference.
        /// </summary>
        [SerializeField] private TimeController _timeController;

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
        ///     Input module getter.
        /// </summary>
        public InputSystemUIInputModule InputModule => _inputModule;

        /// <summary>
        ///     Lives controller getter.
        /// </summary>
        public LivesController LivesController => _livesController;

        /// <summary>
        ///     Level controller getter.
        /// </summary>
        public LevelController LevelController => _levelController;

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
        ///     Time controller getter.
        /// </summary>
        public TimeController TimeController => _timeController;

        /// <summary>
        ///     Initialize the state machine.
        /// </summary>
        private void Awake()
        {
            Instance = this;
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

            if (stateToGoTo == State.Pause || _currentState is PauseState)
            {
                HandlePause(stateToGoTo == State.Pause, stateToGoTo);
                return;
            }
            
            var targetState = _statesDictionary[stateToGoTo];
            if (targetState == _currentState)
            {
                Debug.Log("### - Target state is the same as current state. No need to change.");
                return;
            }
            
            targetState.OnEnterState();
            if (_currentState == null)
            {
                Debug.Log("### - Current state is null. It should only happen on the beginning of the game.");
                _currentState = targetState;
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
            _statesDictionary.Add(State.MainMenu, new MainMenuState(_mainMenu, _levelController));
            _statesDictionary.Add(State.ChooseLevel, new ChooseLevelState(_chooseLevelMenu, _levelController));
            _statesDictionary.Add(State.LeaderBoard, new LeaderBoardState(_leaderBoard, _scoreController));
            _statesDictionary.Add(State.GameActive, new GameActiveState(_livesController, _levelController, _scoreController, _guiController, _cameraController));
            _statesDictionary.Add(State.Pause, new PauseState(_pauseMenu));
            _statesDictionary.Add(State.Result, new ResultState(_gameResultUIController, _livesController, _scoreController, _timeController));

            foreach (var state in _statesDictionary)
            {
                state.Value.Initialize(this);
            }
        }

        /// <summary>
        ///     Special case for pause state.
        ///     Wee need that because we don't want to exit game state when we enter pause state.
        /// </summary>
        /// <param name="entering">Is entering pause state</param>
        /// <param name="targetState">Target state</param>
        private void HandlePause(bool entering, State targetState)
        {
            if (entering)
            {
                // When we enter pause state, we don't want to exit game state.
                // So we just enter pause state and do nothing else.
                _statesDictionary[State.Pause].OnEnterState();
                _currentState = _statesDictionary[State.Pause];
            }
            else
            {
                // If we are exiting pause state, we need to check if we are going back to game state or not.
                // If yes, then we do nothing, as pause state is handling game un-pausing.
                // But if we are not entering game state, we have to exit the game state.
                // This is only valid because we can go either to game state or to menu state.
                // This HAVE TO be refactored if we add more states or if we want to change anything.
                _statesDictionary[State.Pause].OnExitState();
                _currentState = _statesDictionary[targetState];

                if (targetState == State.GameActive)
                    return;
                
                _statesDictionary[State.GameActive].OnExitState();
                _currentState.OnEnterState();
            }
            
        }
    }
}