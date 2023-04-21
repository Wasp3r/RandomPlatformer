using System.ComponentModel;
using UnityEngine;

namespace RandomPlatformer.ScoringSystem
{
    /// <summary>
    ///     This class is just used to show the score in the inspector.
    /// </summary>
    public class DebugScoreInspector : MonoBehaviour
    {
        [SerializeField] private ScoreController _scoreController;

        public string Name;
        public string HighScore;

        [ContextMenu("Save Score")]
        private void SaveScore()
        {
            _scoreController.SaveScore("Player");
        }

        [ContextMenu("Get High Score")]
        private void GetHighsScore()
        {
            var scores = _scoreController.GetLeaderboard();
            if (scores.Count == 0)
            {
                HighScore = "No scores yet";
                return;
            }
            
            var score = scores[0];
            Name = score.UserName;
            HighScore = score.Score.ToString();
        }
        
        [ContextMenu("Wipe Scores")]
        private void WipeScores()
        {
            _scoreController.ClearHighScores();
            GetHighsScore();
        }
    }
}