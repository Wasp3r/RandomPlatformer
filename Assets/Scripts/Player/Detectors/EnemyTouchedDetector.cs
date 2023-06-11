using System;
using RandomPlatformer.MainSceneMachine;
using UnityEngine;

namespace RandomPlatformer.Player.Detectors
{
    /// <summary>
    ///     This class is responsible for detecting the player's touching enemies.
    /// </summary>
    public class EnemyTouchedDetector : PickingController
    {
        [SerializeField] private bool _isImmortal;
    
        /// <summary>
        ///     Lives controller reference.
        /// </summary>
        private LivesController _livesController;

        /// <summary>
        ///     Get reference to the lives controller.
        /// </summary>
        private void OnEnable()
        {
            _livesController = GameStateMachine.Instance.LivesController;
        }

        /// <summary>
        ///     Action to be done when the player touches an enemy.
        /// </summary>
        /// <param name="other">Enemy object.</param>
        protected override void OnPickedUp(GameObject other)
        {
            if (_isImmortal)
            {
                Debug.LogWarning("### - Lost live");
                return;
            }
            _livesController.LoseLive();
        }
    }
}