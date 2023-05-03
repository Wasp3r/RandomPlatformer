using System;
using UnityEngine;

namespace RandomPlatformer.Enemies
{
    /// <summary>
    ///     Projectile class.
    ///     It's responsible for all the projectiles in the game.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        ///     Destroy the projectile on collision.
        /// </summary>
        /// <param name="other">Any object that collides with the projectile.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            Destroy(gameObject);
        }
        
        /// <summary>
        ///     Destroy the projectile on collision.
        /// </summary>
        /// <param name="other">Any object that collides with the projectile.</param>
        private void OnCollisionEnter2D(Collision2D other)
        {
            Destroy(gameObject);
        }
    }
}