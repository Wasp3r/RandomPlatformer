using System;
using RandomPlatformer.LevelSystem;
using RandomPlatformer.MainSceneMachine;
using UnityEngine;

namespace RandomPlatformer.Player.Detectors
{
    /// <summary>
    ///     The controller that handles the finishing of the level.
    /// </summary>
    public class FinishLevelDetector : PickingController
    {
        /// <summary>
        ///     The level controller to finish the level.
        /// </summary>
        private LevelController _levelController;

        /// <summary>
        ///     Assign the level controller.
        /// </summary>
        private void OnEnable()
        {
            _levelController = GameStateMachine.Instance.LevelController;
        }

        /// <summary>
        ///     Detect collision with the finish object and finish the level.
        /// </summary>
        protected override void OnPickedUp(GameObject other)
        {
            _levelController.GoToNextLevel();
        }
    }
}