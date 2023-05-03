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
        [SerializeField] private float _bulletInitialForce;

        /// <summary>
        ///     Bullet shooting direction.
        /// </summary>
        [SerializeField] private Vector2 _shootingDirection;

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
        ///     Is shooting enabled?
        /// </summary>
        private bool _isShooting;

        /// <summary>
        ///     Start shooting.
        /// </summary>
        private void OnEnable()
        {
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
        ///     Shoot the bullet.
        /// </summary>
        [ContextMenu("Shoot")]
        private void ShootBullet()
        {
            var bullet = Instantiate(_bulletPrefab, transform.position + (Vector3) _bulletShootingOffset, Quaternion.identity);
            bullet.AddForce(_shootingDirection * _bulletInitialForce, ForceMode2D.Impulse);
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (!_drawGizmos)
                return;
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + (Vector3) _bulletShootingOffset, 0.1f);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position + (Vector3)_bulletShootingOffset, transform.position + (Vector3)_bulletShootingOffset + (Vector3)_shootingDirection);
        }

#endif
    }
}