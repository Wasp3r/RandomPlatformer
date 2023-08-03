using System.Collections.Generic;
using RandomPlatformer.ScoringSystem;
using RandomPlatformer.UI.Menus;
using UnityEngine;

namespace RandomPlatformer.MainSceneMachine.States
{
    /// <summary>
    ///     Game leaderboard state
    /// </summary>
    public class LeaderBoardState : BaseState
    {
        /// <summary>
        ///     Leaderboard reference.
        /// </summary>
        private readonly LeaderBoard _leaderBoard;

        /// <summary>
        ///     Score controller reference.
        /// </summary>
        private readonly ScoreController _scoreController;
        
        /// <summary>
        ///     Menu background reference.
        ///     We need it to enable it when we enter the main menu state.
        /// </summary>
        private readonly GameObject _menuBackground;

        /// <summary>
        ///     Basic constructor.
        /// </summary>
        /// <param name="leaderBoard">Leaderboard reference</param>
        /// <param name="scoreController">Score controller reference</param>
        /// <param name="menuBackground"></param>
        public LeaderBoardState(LeaderBoard leaderBoard, ScoreController scoreController, GameObject menuBackground)
        {
            _leaderBoard = leaderBoard;
            _scoreController = scoreController;
            _menuBackground = menuBackground;
        }

        /// <inheridoc/>
        public override void OnEnterState()
        {
            _leaderBoard.Enable();
            _menuBackground.SetActive(true);
            _leaderBoard.OnBack += OnCancel;
            _leaderBoard.OnClear += OnClear;
            
            _leaderBoard.ReloadScores(_scoreController.GetLeaderboard());
        }

        /// <inheridoc/>
        public override void OnExitState()
        {
            _leaderBoard.Disable();
            _leaderBoard.OnBack -= OnCancel;
            _leaderBoard.OnClear -= OnClear;
        }

        /// <inheridoc/>
        public override void OnCancel()
        {
            GameStateMachine.GoToState(State.MainMenu);
        }

        /// <summary>
        ///     Handles leaderboard clear.
        /// </summary>
        private void OnClear()
        {
            _scoreController.ClearHighScores();
            _leaderBoard.ReloadScores(new List<ScoreEntry>());
        }
    }
}