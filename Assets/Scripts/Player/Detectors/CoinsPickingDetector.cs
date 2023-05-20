using System;
using RandomPlatformer.MainSceneMachine;
using RandomPlatformer.ScoringSystem;
using UnityEngine;

namespace RandomPlatformer.Player.Detectors
{
    /// <summary>
    ///     The controller that handles the picking up of coins.
    /// </summary>
    public class CoinsPickingDetector : PickingController
    {
        /// <summary>
        ///     Score controller to add points and save the high score.
        /// </summary>
        private ScoreController _scoreController;

        /// <summary>
        ///     Assign the score controller.
        /// </summary>
        private void OnEnable()
        {
            _scoreController = GameStateMachine.Instance.ScoreController;
        }

        /// <summary>
        ///     Add points to the score controller.
        ///     We check if the collided object is a coin.
        /// </summary>
        /// <param name="other">Collided object.</param>
        protected override void OnPickedUp(GameObject other)
        {
            var coin = other.GetComponent<Coin>();
            if (coin == null) 
                return;
            
            _scoreController.AddPoints(coin.Pick());
        }
    }
}