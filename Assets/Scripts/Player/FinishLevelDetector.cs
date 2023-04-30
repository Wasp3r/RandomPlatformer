using System;
using RandomPlatformer.LevelSystem;
using UnityEngine;

namespace RandomPlatformer.Player
{
    /// <summary>
    ///     The controller that handles the finishing of the level.
    /// </summary>
    public class FinishLevelDetector : MonoBehaviour
    {
        /// <summary>
        ///     The layer mask of the objects that the player can finish the level.
        /// </summary>
        [SerializeField] private LayerMask _finishMask;

        /// <summary>
        ///     The level controller to finish the level.
        /// </summary>
        private LevelController _levelController;

        /// <summary>
        ///     Assign the level controller.
        /// </summary>
        private void OnEnable()
        {
            _levelController = GameStateController.Instance.LevelController;
        }

        /// <summary>
        ///     Detect collision with the finish object and finish the level.
        /// </summary>
        /// <param name="other">Collider of the object that collided with the player.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_finishMask != (_finishMask | (1 << other.gameObject.layer))) 
                return;
            
            _levelController.GoToNextLevel();
        }
    }
}