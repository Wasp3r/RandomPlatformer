using System;
using RandomPlatformer.LevelSystem;
using RandomPlatformer.MainSceneMachine;
using RandomPlatformer.Player;
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
        ///     The text that displays the lives.
        /// </summary>
        [SerializeField] private TMP_Text _livesValue;

        /// <summary>
        ///     The score controller to listen to.
        /// </summary>
        private ScoreController _scoreController;

        /// <summary>
        ///     The level controller to listen to.
        /// </summary>
        private LevelController _levelController;

        /// <summary>
        ///     Lives controller to listen to.
        ///     We need it to update the lives value.
        /// </summary>
        private LivesController _livesController;

        /// <summary>
        ///     The score controller to listen to.
        ///     We use that to update the time left.
        /// </summary>
        private TimeController _timeController;
        
        /// <summary>
        ///     We use this to check if the GUI is initialized.
        /// </summary>
        private bool _isInitialized;

        /// <summary>
        ///     We listen to the controllers to show the current values.
        /// </summary>
        private void OnEnable()
        {
            Initialize();
            _scoreController.OnScoreChanged += OnUpdateScore;
            _levelController.OnLevelLoaded += OnLevelLoaded;
            _livesController.OnLivesChanged += OnLivesChanged;
            _timeController.OnTimeLeftChanged += UpdateTimeLeft;
            _livesValue.text = _livesController.Lives.ToString();
        }

        /// <summary>
        ///     We stop listening to the controllers to not update the values when the GUI is not active.
        /// </summary>
        private void OnDisable()
        {
            _timeController.OnTimeLeftChanged -= UpdateTimeLeft;
            _scoreController.OnScoreChanged -= OnUpdateScore;
            _levelController.OnLevelLoaded -= OnLevelLoaded;
            _livesController.OnLivesChanged -= OnLivesChanged;
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
        private void UpdateTimeLeft(float timeLeft)
        {
            var timespan = TimeSpan.FromSeconds(timeLeft);
            _timeLeftValue.text = timespan.ToString("mm':'ss");
        }

        /// <summary>
        ///     Updates the score text.
        ///     We need it to show the current score value.
        /// </summary>
        /// <param name="score">The current score.</param>
        private void OnUpdateScore(int score)
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
        ///     When the lives are changed we update the lives value.
        /// </summary>
        private void OnLivesChanged(int currentLives)
        {
            _livesValue.text = currentLives.ToString();
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

        /// <summary>
        ///     Initializes the GUI.
        /// </summary>
        private void Initialize()
        {
            if (_isInitialized)
                return;
            
            _isInitialized = true;
            _scoreController = GameStateMachine.Instance.ScoreController;
            _levelController = GameStateMachine.Instance.LevelController;
            _livesController = GameStateMachine.Instance.LivesController;
            _timeController = GameStateMachine.Instance.TimeController;
        }
    }
}