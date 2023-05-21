using RandomPlatformer.LevelSystem;
using RandomPlatformer.Player;
using RandomPlatformer.ScoringSystem;
using RandomPlatformer.UI.Menus;
using UnityEngine;

namespace RandomPlatformer.MainSceneMachine.States
{
    /// <summary>
    ///     The result state.
    /// </summary>
    public class ResultState : BaseState
    {
        /// <summary>
        ///     The game result UI controller.
        /// </summary>
        private readonly GameResultUIController _gameResultUIController;
        
        /// <summary>
        ///     Lives controller reference.
        ///     We need it to check how many lives the player has when entering the state.
        ///     This way we can recognize if the player won or lost.
        /// </summary>
        private readonly LivesController _livesController;
        
        /// <summary>
        ///     Score controller reference.
        ///     We need it to read and save the current score.
        /// </summary>
        private readonly ScoreController _scoreController;
        
        /// <summary>
        ///     Time controller reference.
        ///     We need it to read time left and add it to the score.
        /// </summary>
        private readonly TimeController _timeController;

        /// <summary>
        ///     Basic constructor.
        /// </summary>
        /// <param name="gameResultUIController">The game result UI controller.</param>
        /// <param name="livesController">Lives controller reference.</param>
        /// <param name="scoreController">Score controller reference.</param>
        public ResultState(GameResultUIController gameResultUIController, LivesController livesController, ScoreController scoreController, TimeController timeController)
        {
            _gameResultUIController = gameResultUIController;
            _livesController = livesController;
            _scoreController = scoreController;
            _timeController = timeController;
        }

        /// <inheridoc/>
        public override void OnEnterState()
        {
            var won = _livesController.Lives > 0 && _timeController.TimeLeft > 0;
            _gameResultUIController.OnBack += SaveScoreAndGoToMainMenu;
            _gameResultUIController.ShowResult(won, GetScore(won));
            _gameResultUIController.Enable();
        }

        /// <inheridoc/>
        public override void OnExitState()
        {
            _gameResultUIController.OnBack -= SaveScoreAndGoToMainMenu;
            _gameResultUIController.Disable();
        }

        /// <inheridoc/>
        public override void OnCancel()
        {
            SaveScoreAndGoToMainMenu();
        }

        private string GetScore(bool won)
        {
            if (!won)
            {
                return _scoreController.CurrentScore.ToString();
            }
            
            _scoreController.AddPoints(Mathf.RoundToInt(_timeController.TimeLeft));
            return _scoreController.CurrentScore.ToString();
        }

        private void SaveScoreAndGoToMainMenu(string userName = null)
        {
            userName ??= "Anonymous Player";
            _scoreController.SaveScore(userName);
            GameStateMachine.GoToState(State.MainMenu);
        }
    }
}