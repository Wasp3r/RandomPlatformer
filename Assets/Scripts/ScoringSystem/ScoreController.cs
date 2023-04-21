using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace RandomPlatformer.ScoringSystem
{
    /// <summary>
    ///     The controller for the scores.
    ///     We need it to keep track of the current score, save it and read the leaderboard.
    /// </summary>
    public class ScoreController : MonoBehaviour
    {
        /// <summary>
        ///     The current score.
        /// </summary>
        public int CurrentScore { get; private set; }
        
        /// <summary>
        ///     Adds the score to the current score.
        ///     We need it to add scores when player do something.
        /// </summary>
        /// <param name="score">Score to be added.</param>
        public void AddPoints(int score)
        {
            CurrentScore += score;
            Debug.Log($"### - Got points ({score}. Current score: {CurrentScore}");
        }
        
        /// <summary>
        ///     Resets the score to 0.
        ///     We need it to reset the score when the player dies.
        /// </summary>
        public void ResetScore()
        {
            Debug.Log("### - Resetting score");
            CurrentScore = 0;
        }
        
        /// <summary>
        ///     Saves the score to the player prefs.
        ///     We need it to keep the score and the leaderboard between the game sessions.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        public void SaveScore(string userName)
        {
            Debug.Log("### - Getting scores");
            var scores = GetLeaderboard();
            var userScore = scores.Find(x => x.UserName == userName);
            if (userScore != null)
            {
                if (CurrentScore <= userScore.Score)
                {
                    Debug.Log("### - New high score lower or equal to the old one. Not saving.");
                    return;
                }
            }
            
            scores.Add(new ScoreEntry(CurrentScore, userName, DateTime.Now));
            PlayerPrefs.SetString("Scores", JsonConvert.SerializeObject(scores));
        }
        
        /// <summary>
        ///     Gets the leaderboard from the player prefs.
        ///     We need it to show the leaderboard or to save the score.
        /// </summary>
        /// <returns>The leaderboard.</returns>
        public List<ScoreEntry> GetLeaderboard()
        {
            Debug.Log("### - Getting scores");
            var scores = PlayerPrefs.GetString("Scores", "");
            var scoreEntries = JsonConvert.DeserializeObject<List<ScoreEntry>>(scores) ?? new List<ScoreEntry>();
            scoreEntries.Sort((x, y) => y.Score.CompareTo(x.Score));
            return scoreEntries;
        }

        /// <summary>
        ///     Clears the leaderboard.
        ///     We need it to clear the leaderboard when the player wants to.
        /// </summary>
        public void ClearHighScores()
        {
            PlayerPrefs.SetString("Scores", "");
        }
    }
}