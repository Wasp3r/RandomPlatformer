using System;
using UnityEngine;

namespace RandomPlatformer.Player
{
    /// <summary>
    ///     This class is responsible for managing the player's animations.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour
    {
        /// <summary>
        ///     Animator reference.
        /// </summary>
        [SerializeField] private Animator _animator;

        /// <summary>
        ///     Jump smoke prefab.
        /// </summary>
        [SerializeField] private JumpSmoke _jumpSmokePrefab;
        
        /// <summary>
        ///     Movement animation hash.
        /// </summary>
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        /// <summary>
        ///     Jump animation hash.
        /// </summary>
        private static readonly int Jump = Animator.StringToHash("Jump");
        
        /// <summary>
        ///     Jump finish animation hash.
        /// </summary>
        private static readonly int JumpFinish = Animator.StringToHash("JumpFinish");

        /// <summary>
        ///     Death animation hash.
        /// </summary>
        private static readonly int Death = Animator.StringToHash("Death");

        /// <summary>
        ///     Triggered when the death animation finishes.
        /// </summary>
        public event Action OnDeathAnimationFinished;

        /// <summary>
        ///     Triggers jumping animation.
        /// </summary>
        public void TriggerJump()
        {
            _animator.SetTrigger(Jump);
            Instantiate(_jumpSmokePrefab, transform.position, Quaternion.identity);
        }

        /// <summary>
        ///     Triggers jumping finish animation.
        /// </summary>
        public void TriggerJumpFinish()
        {
            _animator.SetTrigger(JumpFinish);
        }
        
        /// <summary>
        ///     Changes the moving animation state.
        /// </summary>
        /// <param name="movingState">The new moving state.</param>
        public void TriggerIsMoving(bool movingState)
        {
            _animator.SetBool(IsMoving, movingState);
        }

        /// <summary>
        ///     Triggers the death animation.
        /// </summary>
        public void TriggerDeath()
        {
            _animator.SetTrigger(Death);
        }

        /// <summary>
        ///     This method is called by the animation event.
        ///     We need it so we can trigger the death animation action.
        /// </summary>
        private void DeathAnimationFinished()
        {
            OnDeathAnimationFinished?.Invoke();
        }
    }
}