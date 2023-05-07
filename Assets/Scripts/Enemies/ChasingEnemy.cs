using System;
using System.Linq;
using UnityEngine;

namespace RandomPlatformer.Enemies
{
    /// <summary>
    ///     This class is responsible for the chasing enemies. They chase the player within a certain range.
    /// </summary>
    public class ChasingEnemy : WalkingEnemy
    {
        /// <summary>
        ///     Movement speed when chasing the player.
        /// </summary>
        [SerializeField] private float _chasingMovementSpeed;

        /// <summary>
        ///     Player layer mask.
        /// </summary>
        [SerializeField] private LayerMask _playerMask;

        /// <summary>
        ///     Ground layer mask.
        /// </summary>
        [SerializeField] private LayerMask _groundLayerMask;

        /// <summary>
        ///     Player position.
        /// </summary>
        private Vector2 _playerPosition;

        protected override void OnEnable()
        {
            base.OnEnable();
            FindEdges();
        }

        protected override void Update()
        {
            if (!CheckForPlayer())
            {
                base.Update();
                return;
            }
            
            if (_playerPosition == Vector2.zero)
                return;
            
            _localTransform.position = Vector2.MoveTowards(_localTransform.position, _playerPosition, _chasingMovementSpeed * Time.deltaTime);
        }
        
        /// <summary>
        ///     Checks if the player is within the range.
        /// </summary>
        /// <returns>True if the player is within the range.</returns>
        private bool CheckForPlayer()
        {
            _playerPosition = Physics2D.Raycast(_localTransform.position, Vector2.right, _maxDistance, _playerMask).point;
            if (_playerPosition == Vector2.zero)
            {
                _playerPosition = Physics2D.Raycast(_localTransform.position, Vector2.left, _maxDistance, _playerMask).point;
            }
            
            if (_playerPosition == Vector2.zero)
                return false;
            
            _playerPosition = new Vector2(_playerPosition.x, _localTransform.position.y);
            if (_playerPosition.x < _positionA.x || _playerPosition.x > _positionB.x)
                return false;
            
            return _playerPosition != Vector2.zero;
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (!_drawGizmos) 
                return;

            if (_playerPosition == Vector2.zero)
                return;
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, _playerPosition);
        }
#endif
    }
}