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
        /// <summary>
        ///     Player animation controller reference.
        ///     We need it to trigger the death animation.
        /// </summary>
        [SerializeField] private PlayerAnimationController _animationController;

        /// <summary>
        ///     Movement controller reference.
        ///     We need it to lock the movement when the player dies.
        /// </summary>
        [SerializeField] private MovementController _movementController;

        [SerializeField] private bool _isImmortal;

        /// <summary>
        ///     Lives controller reference.
        /// </summary>
        private LivesController _livesController;

        private void Start()
        {
            _animationController.OnDeathAnimationFinished += LoseLive;
        }

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
#if UNITY_EDITOR
                Debug.LogWarning("### - Lost live");
#endif
                return;
            }
            
            SoundController.Instance.PlayDeathSound();
            _movementController.UpdateMovementState(false);
            _animationController.TriggerDeath();
            _isImmortal = true;
        }
        
        /// <summary>
        ///     We use it to take the player's live and make him mortal again.
        /// </summary>
        private void LoseLive()
        {
            _movementController.ResetPosition();
            _livesController.LoseLive();
            _isImmortal = false;
        }
    }
}