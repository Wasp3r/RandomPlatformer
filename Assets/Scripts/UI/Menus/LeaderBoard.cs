using System;
using RandomPlatformer.ScoringSystem;
using RandomPlatformer.UI.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RandomPlatformer.UI.Menus
{
    /// <summary>
    ///     The leaderboard.
    ///     This class is responsible for the leaderboard UI. It shows the top 10 players.
    /// </summary>
    public class LeaderBoard : UIFadable
    {
        /// <summary>
        ///     Container for the leaderboard items.
        /// </summary>
        [SerializeField] private VerticalLayoutGroup _container;

        /// <summary>
        ///     Clear button to remove all the scores.
        /// </summary>
        [SerializeField] private Button _clearButton;

        /// <summary>
        ///     Back button to go back to the main menu.
        /// </summary>
        [SerializeField] private Button _backButton;
        
        /// <summary>
        ///     Leaderboard item prefab.
        /// </summary>
        [SerializeField] private LeaderBoardItem _leaderBoardItemPrefab;

        /// <summary>
        ///     Max high scores to show.
        /// </summary>
        [SerializeField] private int _containerSize;

        /// <summary>
        ///     Score controller to get the scores.
        /// </summary>
        private ScoreController _scoreController;

        /// <summary>
        ///     Is the leaderboard initialized.
        /// </summary>
        private bool _isInitialized;

        /// <summary>
        ///     Listen to button events.
        /// </summary>
        private void OnEnable()
        {
            _clearButton.onClick.AddListener(ClearHighScores);
            _backButton.onClick.AddListener(GoToMenu);
        }

        /// <summary>
        ///     Stop listening to button events.
        /// </summary>
        private void OnDisable()
        {
            _clearButton.onClick.RemoveListener(ClearHighScores);
            _backButton.onClick.RemoveListener(GoToMenu);
        }

        /// <summary>
        ///     Fades in the leaderboard and reloads the scores.
        ///     We need it to show the leaderboard and load the scores.
        /// </summary>
        public void Enable()
        {
            FadeIn();
            ReloadScores();
        }

        /// <summary>
        ///     Fades out the leaderboard.
        /// </summary>
        public void Disable()
        {
            FadeOut();
        }

        /// <summary>
        ///     Reloads the scores.
        ///     We need it to show updated scores.
        /// </summary>
        private void ReloadScores()
        {
            ClearScoresContainer();
            if (!_isInitialized)
            {
                Initialize();
            }
            
            var scores = _scoreController.GetLeaderboard();
            for (var i = 0; i < _containerSize; i++)
            {
                var score = scores[i];
                var leaderBoardItem = Instantiate(_leaderBoardItemPrefab, _container.transform);
                leaderBoardItem.SetScoreEntry(score, i + 1);
            }
        }

        /// <summary>
        ///     Clears the scores container.
        ///     We need to remove all the leaderboard items from the container to populate it with new ones.
        /// </summary>
        private void ClearScoresContainer()
        {
            for (var i = _container.transform.childCount - 1; i >= 0; i--)
            {
                var child = _container.transform.GetChild(i);
                if (Application.isPlaying)
                    Destroy(child.gameObject);
#if UNITY_EDITOR
                else
                    DestroyImmediate(child.gameObject);
#endif
            }
        }
        
        /// <summary>
        ///     Initializes the leaderboard.
        ///     We need it to get the score controller.
        /// </summary>
        private void Initialize()
        {
            _scoreController = GameController.Instance.ScoreController;
            _isInitialized = true;
        }
        
        /// <summary>
        ///     Go back to the main menu.
        /// </summary>
        private void GoToMenu()
        {
            Disable();
            GameController.Instance.UpdateGameState(GameState.Menu);
        }

        /// <summary>
        ///     Removes all the high scores and reloads the leaderboard.
        /// </summary>
        private void ClearHighScores()
        {
            _scoreController.ClearHighScores();
            ReloadScores();
        }
    }
}