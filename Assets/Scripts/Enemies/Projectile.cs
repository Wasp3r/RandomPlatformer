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
        ///     Projectile life time.
        /// </summary>
        [SerializeField] private float _lifeTime = 3;
        
        /// <summary>
        ///     Time passed.
        /// </summary>
        private float _timer;
        
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

        /// <summary>
        ///     Update the timer and destroy the projectile if it's time to do so.
        /// </summary>
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _lifeTime)
                Destroy(gameObject);
        }
    }
}