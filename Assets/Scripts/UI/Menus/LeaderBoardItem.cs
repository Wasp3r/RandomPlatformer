using RandomPlatformer.ScoringSystem;
using TMPro;
using UnityEngine;

namespace RandomPlatformer.UI.Menus
{
    /// <summary>
    ///     The leaderboard item.
    ///     This is a container for the leaderboard item.
    /// </summary>
    public class LeaderBoardItem : MonoBehaviour
    {
        /// <summary>
        ///     LP text.
        /// </summary>
        [SerializeField] private TMP_Text _lp;
        
        /// <summary>
        ///     Name text.
        /// </summary>
        [SerializeField] private TMP_Text _name;
        
        /// <summary>
        ///     Score text.
        /// </summary>
        [SerializeField] private TMP_Text _score;

        /// <summary>
        ///     Set the score entry.
        ///     It is used to set all the values for the leaderboard item.
        /// </summary>
        /// <param name="scoreEntry"></param>
        /// <param name="index"></param>
        public void SetScoreEntry(ScoreEntry scoreEntry, int index)
        {
            _lp.text = $"{index.ToString()}. ";
            _name.text = scoreEntry.UserName;
            _score.text = scoreEntry.Score.ToString();
        }
    }
}