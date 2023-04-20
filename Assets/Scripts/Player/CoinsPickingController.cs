using System;
using RandomPlatformer.ScoringSystem;
using UnityEngine;

namespace RandomPlatformer.Player
{
    /// <summary>
    ///     The controller that handles the picking up of coins.
    /// </summary>
    public class CoinsPickingController : MonoBehaviour
    {
        /// <summary>
        ///     Score controller to add points and save the high score.
        /// </summary>
        [SerializeField] private ScoreController _scoreController;

        /// <summary>
        ///     The layer mask of the objects that the player can pick up.
        /// </summary>
        [SerializeField] private LayerMask _coinMask;

        /// <summary>
        ///     The collider to pick up coins.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            if (_coinMask != (_coinMask | (1 << collision.gameObject.layer))) 
                return;
            
            var coin = collision.collider.GetComponent<Coin>();
            if (coin == null) 
                return;
            
            _scoreController.AddPoints(coin.Pick());
        }
    }
}