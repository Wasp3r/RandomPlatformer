namespace RandomPlatformer.ScoringSystem
{
    /// <summary>
    ///     A class that represents a score entry.
    /// </summary>
    public class ScoreEntry
    {
        /// <summary>
        ///     The name of the player.
        /// </summary>
        public string UserName { get; private set; }
        
        /// <summary>
        ///     The score of the player.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        ///     The score entry constructor.
        /// </summary>
        /// <param name="score">Score of the player.</param>
        /// <param name="userName">Name of the player.</param>
        public ScoreEntry(int score, string userName)
        {
            Score = score;
            UserName = userName;
        }
    }
}