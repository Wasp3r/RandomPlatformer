using System;
using RandomPlatformer.ScoringSystem;
using RandomPlatformer.UI.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RandomPlatformer.UI.Menus
{
    /// <summary>
    ///     The game result controller is responsible for showing the game result.
    /// </summary>
    public class GameResultUIController : UIFadable
    {
        /// <summary>
        ///     The score controller.
        /// </summary>
        [SerializeField] private ScoreController _scoreController;
        
        /// <summary>
        ///     The result text.
        ///     Here is where we show the user if he won or lost.
        /// </summary>
        [SerializeField] private TMP_Text _resultText;

        /// <summary>
        ///     The score text.
        /// </summary>
        [SerializeField] private TMP_Text _score;

        /// <summary>
        ///     The user name input field.
        /// </summary>
        [SerializeField] private TMP_InputField _userName;

        /// <summary>
        ///     The back button.
        /// </summary>
        [SerializeField] private Button _backButton;

        /// <summary>
        ///     Listen to events.
        /// </summary>
        private void OnEnable()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _userName.text = "";
        }

        /// <summary>
        ///     Stop listening to events.
        /// </summary>
        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
        }

        /// <summary>
        ///     Show the game result.
        /// </summary>
        /// <param name="won">Is the game won or lost.</param>
        public void ShowResult(bool won)
        {
            _resultText.text = won ? "You Won!" : "You Lost!";
            _score.text = _scoreController.CurrentScore.ToString();
            Enable();
        }

        private void OnBackButtonClicked()
        {
            _scoreController.SaveScore(_userName.text);
            _scoreController.ResetScore();
            Disable();
            GameStateController.Instance.UpdateGameState(GameState.Menu);
        }
    }
}