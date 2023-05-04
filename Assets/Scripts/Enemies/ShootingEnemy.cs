using System;
using UnityEngine;

namespace RandomPlatformer.Enemies
{
    /// <summary>
    ///     Shooting enemy.
    ///     It's responsible for all the shooting enemies in the game.
    /// </summary>
    public class ShootingEnemy : MonoBehaviour
    {
        /// <summary>
        ///     Bullet prefab.
        /// </summary>
        [SerializeField] private Rigidbody2D _bulletPrefab;

        /// <summary>
        ///     Bullet initial speed.
        /// </summary>
        [SerializeField] private float _bulletSpeed = 10;

        /// <summary>
        ///     Bullet shooting offset.
        /// </summary>
        [SerializeField] private Vector2 _bulletShootingOffset;

        /// <summary>
        ///     Time between shots.
        /// </summary>
        [SerializeField] private float _shootingInterval = 3f;

#if UNITY_EDITOR
        [SerializeField] private bool _drawGizmos;
#endif

        /// <summary>
        ///     Bullet shooting direction.
        /// </summary>
        private Vector2 _shootingDirection;
        
        /// <summary>
        ///     Is shooting enabled?
        /// </summary>
        private bool _isShooting;

        /// <summary>
        ///     Time passed since the last shot.
        /// </summary>
        private float _timeSinceLastShot;

        /// <summary>
        ///     Start shooting.
        /// </summary>
        private void OnEnable()
        {
            _shootingDirection = transform.right;
            _isShooting = true;
        }
        
        /// <summary>
        ///     Stop shooting.
        /// </summary>
        private void OnDisable()
        {
            _isShooting = false;
        }

        /// <summary>
        ///     Update the timer and shoot the bullet if it's time to do so.
        /// </summary>
        private void Update()
        {
            if (!_isShooting)
                return;

            _timeSinceLastShot += Time.deltaTime;
            if (!(_timeSinceLastShot >= _shootingInterval)) 
                return;
            
            _timeSinceLastShot = 0;
            ShootBullet();
        }

        /// <summary>
        ///     Shoot the bullet.
        /// </summary>
        private void ShootBullet()
        {
            var bullet = Instantiate(_bulletPrefab, transform.position + (Vector3) _bulletShootingOffset, Quaternion.identity);
            bullet.transform.right = _shootingDirection;
            bullet.velocity = _bulletSpeed * _shootingDirection;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (!_drawGizmos)
                return;
            
            _shootingDirection = transform.right;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + (Vector3) _bulletShootingOffset, 0.1f);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position + (Vector3)_bulletShootingOffset, transform.position + (Vector3)_bulletShootingOffset + (Vector3)_shootingDirection);
        }

#endif
    }
}