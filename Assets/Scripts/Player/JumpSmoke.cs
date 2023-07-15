using System;
using UnityEngine;

namespace RandomPlatformer.Player
{
    /// <summary>
    ///     This class is responsible for managing the player's jump smoke.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class JumpSmoke : MonoBehaviour
    {
        /// <summary>
        ///     Animator reference responsible for the smoke animation.
        /// </summary>
        [SerializeField] private Animator _animator;

        /// <summary>
        ///     Triggered when the smoke animation finishes.
        /// </summary>
        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}