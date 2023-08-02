using System;
using RandomPlatformer.MainSceneMachine;
using RandomPlatformer.UI.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RandomPlatformer.UI.Menus
{
    /// <summary>
    ///     The main menu.
    ///     This class is responsible for the main menu UI.
    /// </summary>
    public class MainMenu : UIFadable
    {
        /// <summary>
        ///     Start button to start the game.
        /// </summary>
        [SerializeField] private Button _startButton;
        
        /// <summary>
        ///     Choose level button to choose the level.
        /// </summary>
        [SerializeField] private Button _chooseLevelButton;
        
        /// <summary>
        ///     Leaderboard button to show the leaderboard.
        /// </summary>
        [SerializeField] private Button _leaderboardButton;

        /// <summary>
        ///     Credits button to show the credits.
        /// </summary>
        [SerializeField] private Button _creditsButton;
        
        /// <summary>
        ///     Exit button to exit the game.
        /// </summary>
        [SerializeField] private Button _exitButton;

        public event Action OnStartGame;
        public event Action OnChooseLevel;
        public event Action OnLeaderboard;
        public event Action OnCredits;
        public event Action OnExitGame;

        private void OnEnable()
        {
            _startButton.onClick.AddListener(StartGame);
            _chooseLevelButton.onClick.AddListener(ChooseLevel);
            _leaderboardButton.onClick.AddListener(Leaderboard);
            _creditsButton.onClick.AddListener(Credits);
            _exitButton.onClick.AddListener(ExitGame);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(StartGame);
            _chooseLevelButton.onClick.RemoveListener(ChooseLevel);
            _leaderboardButton.onClick.RemoveListener(Leaderboard);
            _creditsButton.onClick.RemoveListener(Credits);
            _exitButton.onClick.RemoveListener(ExitGame);
        }

        private void StartGame()
        {
            OnStartGame?.Invoke();
        }

        private void ChooseLevel()
        {
            OnChooseLevel?.Invoke();
        }
        
        private void Leaderboard()
        {
            OnLeaderboard?.Invoke();
        }
        
        private void Credits()
        {
            OnCredits?.Invoke();
        }
        
        private void ExitGame()
        {
            OnExitGame?.Invoke();
        }
    }
}