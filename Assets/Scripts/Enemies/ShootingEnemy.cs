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
        ///     Bullet shooting point.
        /// </summary>
        [SerializeField] private Transform _shootingPoint;

        /// <summary>
        ///     Bullet initial speed.
        /// </summary>
        [SerializeField] private float _bulletSpeed = 10;

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
        ///     Bullet shooting position.
        /// </summary>
        private Vector2 _bulletShootingPosition;
        
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
            _shootingDirection = _shootingPoint.right;
            _bulletShootingPosition = _shootingPoint.position;
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
            var bullet = Instantiate(_bulletPrefab, _bulletShootingPosition, Quaternion.identity);
            bullet.transform.right = _shootingDirection;
            bullet.velocity = _bulletSpeed * _shootingDirection;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (!_drawGizmos)
                return;
            
            _shootingDirection = _shootingPoint.right;
            _bulletShootingPosition = _shootingPoint.position;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_bulletShootingPosition, 0.1f);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_bulletShootingPosition, _bulletShootingPosition + _shootingDirection);
        }

#endif
    }
}