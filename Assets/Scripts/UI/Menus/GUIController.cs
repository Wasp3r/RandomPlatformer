using System;
using RandomPlatformer.LevelSystem;
using RandomPlatformer.ScoringSystem;
using RandomPlatformer.UI.Generic;
using TMPro;
using UnityEngine;

namespace RandomPlatformer.UI.Menus
{
    /// <summary>
    ///     This class is responsible for the main GUI.
    /// </summary>
    public class GUIController : MonoBehaviour
    {
        /// <summary>
        ///     The score controller to listen to.
        /// </summary>
        [SerializeField] private ScoreController _scoreController;

        /// <summary>
        ///     The level controller to listen to.
        /// </summary>
        [SerializeField] private LevelController _levelController;

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
            _levelController.OnLevelLoaded += OnLevelLoaded;
        }
        
        /// <summary>
        ///     We stop listening to the controllers to not update the values when the GUI is not active.
        /// </summary>
        private void OnDisable()
        {
            _scoreController.ScoreChanged -= UpdateScore;
            _levelController.OnLevelLoaded -= OnLevelLoaded;
        }

        /// <summary>
        ///     Enables the GUI.
        /// </summary>
        public void Enable()
        {
            ResetAll();
            gameObject.SetActive(true);
        }

        /// <summary>
        ///     Disables the GUI.
        /// </summary>
        public void Disable()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        ///     Updates the time left.
        /// </summary>
        /// <param name="timeLeft">Time left in seconds.</param>
        public void UpdateTimeLeft(float timeLeft)
        {
            var timespan = TimeSpan.FromSeconds(timeLeft);
            _timeLeftValue.text = timespan.ToString("mm':'ss");
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

        /// <summary>
        ///     Updates the level number.
        /// </summary>
        /// <param name="level">The current level.</param>
        private void UpdateLevel(int level)
        {
            _levelValue.text = level.ToString();
        }

        /// <summary>
        ///     When the level is loaded we update the level number.
        /// </summary>
        /// <param name="loadedLevel">The loaded level.</param>
        private void OnLevelLoaded(LevelInstance loadedLevel)
        {
            UpdateLevel(loadedLevel.LevelNumber);
        }

        /// <summary>
        ///     Resets all the values.
        /// </summary>
        private void ResetAll()
        {
            _scoreValue.text = "0";
            _levelValue.text = "1";
            _timeLeftValue.text = "00:00";
        }
    }
}