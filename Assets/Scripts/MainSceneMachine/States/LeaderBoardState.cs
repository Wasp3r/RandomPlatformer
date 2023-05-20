using System.Collections.Generic;
using RandomPlatformer.ScoringSystem;
using RandomPlatformer.UI.Menus;

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
        ///     Basic constructor.
        /// </summary>
        /// <param name="leaderBoard">Leaderboard reference</param>
        /// <param name="scoreController">Score controller reference</param>
        public LeaderBoardState(LeaderBoard leaderBoard, ScoreController scoreController)
        {
            _leaderBoard = leaderBoard;
            _scoreController = scoreController;
        }

        /// <inheridoc/>
        public override void OnEnterState()
        {
            _leaderBoard.Enable();
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