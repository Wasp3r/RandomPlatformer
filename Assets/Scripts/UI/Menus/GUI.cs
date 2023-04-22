using System;
using RandomPlatformer.ScoringSystem;
using TMPro;
using UnityEngine;

namespace RandomPlatformer.UI.Menus
{
    /// <summary>
    ///     This class is responsible for the main GUI.
    /// </summary>
    public class GUI : MonoBehaviour
    {
        /// <summary>
        ///     The score controller to listen to.
        /// </summary>
        [SerializeField] private ScoreController _scoreController;

        /// <summary>
        ///     The text that displays the score.
        /// </summary>
        [SerializeField] private TMP_Text _levelValue;
        
        /// <summary>
        ///     The text that displays the score.
        /// </summary>
        [SerializeField] private TMP_Text _scoreValue;

        /// <summary>
        ///     The text that displays the time left.
        /// </summary>
        [SerializeField] private TMP_Text _timeLeftValue;

        /// <summary>
        ///     We listen to the controllers to show the current values.
        /// </summary>
        private void OnEnable()
        {
            _scoreController.ScoreChanged += UpdateScore;
        }
        
        /// <summary>
        ///     We stop listening to the controllers to not update the values when the GUI is not active.
        /// </summary>
        private void OnDisable()
        {
            _scoreController.ScoreChanged -= UpdateScore;
        }
        
        /// <summary>
        ///     Updates the score text.
        ///     We need it to show the current score value.
        /// </summary>
        /// <param name="score">The current score.</param>
        private void UpdateScore(int score)
        {
            _scoreValue.text = score.ToString();
        }
    }
}