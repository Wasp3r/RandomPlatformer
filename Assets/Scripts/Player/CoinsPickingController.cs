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
        ///     The layer mask of the objects that the player can pick up.
        /// </summary>
        [SerializeField] private LayerMask _coinMask;
        
        /// <summary>
        ///     Score controller to add points and save the high score.
        /// </summary>
        private ScoreController _scoreController;

        private void OnEnable()
        {
            _scoreController = GameController.Instance.ScoreController;
        }

        /// <summary>
        ///     The collider to pick up coins.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_coinMask != (_coinMask | (1 << other.gameObject.layer))) 
                return;
            
            var coin = other.GetComponent<Coin>();
            if (coin == null) 
                return;
            
            _scoreController.AddPoints(coin.Pick());
        }
    }
}