using System;
using UnityEngine;

namespace RandomPlatformer.Player.Detectors
{
    /// <summary>
    ///     This class is responsible for detecting the player's picking lives.
    /// </summary>
    public class LivePickingDetector : PickingController
    {
        /// <summary>
        ///     Lives controller reference.
        /// </summary>
        private LivesController _livesController;

        /// <summary>
        ///     Get reference to the lives controller.
        /// </summary>
        private void OnEnable()
        {
            _livesController = GameStateController.Instance.LivesController;
        }

        /// <summary>
        ///     Action to be done when the player picks up a live.
        /// </summary>
        /// <param name="other">Live giving object.</param>
        protected override void OnPickedUp(GameObject other)
        {
            _livesController.AddLive();
            Destroy(other);
        }
    }
}