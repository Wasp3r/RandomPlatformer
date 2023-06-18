using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RandomPlatformer.ScoringSystem
{
    /// <summary>
    ///     The coin that the player can pick up.
    /// </summary>
    public class Coin : MonoBehaviour
    {
        /// <summary>
        ///     The animator of the coin.
        ///     Used to put coin into floating and spinning animation.
        /// </summary>
        [SerializeField] private Animator _animator;
        
        /// <summary>
        ///     The number of points the coin is worth.
        /// </summary>
        [SerializeField] private int _points;

        private static readonly int CycleOffset = Animator.StringToHash("CycleOffset");
        private static readonly int SpinTrigger = Animator.StringToHash("Spin");
        private static readonly int FloatTrigger = Animator.StringToHash("Float");

        private void Start()
        {
            _animator.SetFloat(CycleOffset, Random.Range(0f,1f));
            _animator.SetTrigger(SpinTrigger);
            _animator.SetTrigger(FloatTrigger);
        }

        /// <summary>
        ///     Picks up the coin and destroys it.
        ///     We need it to be able to pick up the coin from the outside.
        /// </summary>
        /// <returns>The number of points the coin is worth.</returns>
        public int Pick()
        {
            Destroy(gameObject);
            return _points;
        }
    }
}