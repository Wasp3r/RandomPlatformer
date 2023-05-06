using System;
using UnityEngine;

namespace RandomPlatformer.Enemies
{
    /// <summary>
    ///     This class is responsible for the walking enemies. They walk from point A to point B and back.
    ///     You can also make it walk from edge to edge.
    /// </summary>
    public class WalkingEnemy : MonoBehaviour
    {
        /// <summary>
        ///     Enemy movement speed.
        /// </summary>
        [SerializeField] private float _movementSpeed;

        /// <summary>
        ///     Point A position offset.
        /// </summary>
        [SerializeField] private float _positionAOffset;

        /// <summary>
        ///     Point B position offset.
        /// </summary>
        [SerializeField] private float _positionBOffset;
        
        /// <summary>
        ///     Enemy moving direction.
        /// </summary>
        [SerializeField] private bool _movingToA;

        /// <summary>
        ///     Max distance from the starting position.
        /// </summary>
        [SerializeField] private float _maxDistance;

        /// <summary>
        ///     Enemy width.
        ///     We need it to find out if the enemy is at the edge.
        /// </summary>
        [SerializeField] private float _enemyWidth;

#if UNITY_EDITOR
        [SerializeField] private bool _drawGizmos;
#endif

        /// <summary>
        ///     Is the enemy moving?
        /// </summary>
        private bool _isMoving;

        /// <summary>
        ///     Enemy local transform.
        /// </summary>
        private Transform _localTransform;
        
        /// <summary>
        ///     Point A position.
        /// </summary>
        private Vector2 _positionA;
        
        /// <summary>
        ///     Point B position.
        /// </summary>
        private Vector2 _positionB;

        /// <summary>
        ///     Assign local transform.
        /// </summary>
        private void Awake()
        {
            _localTransform = transform;
            var position = _localTransform.position;

            _positionA = new Vector2(position.x + _positionAOffset, position.y);
            _positionB = new Vector2(position.x + _positionBOffset, position.y);
        }

        /// <summary>
        ///     Start enemy movement.
        /// </summary>
        private void OnEnable()
        {
            _isMoving = true;
        }
        
        /// <summary>
        ///     Stop enemy movement.
        /// </summary>
        private void OnDisable()
        {
            _isMoving = false;
        }

        /// <summary>
        ///     Update enemy position.
        /// </summary>
        private void Update()
        {
            if (!_isMoving)
                return;

            MoveTowardsDestination();
        }

        /// <summary>
        ///     Move the enemy towards the destination point.
        /// </summary>
        private void MoveTowardsDestination()
        {
            var targetPosition = _movingToA ? _positionA : _positionB;
            var position = _localTransform.position;
            
            if (Vector2.Distance(position, targetPosition) < 0.1f)
            {
                _movingToA = !_movingToA;
                return;
            }
            
            _localTransform.position = Vector2.MoveTowards(position, targetPosition, _movementSpeed * Time.deltaTime);
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (!_drawGizmos)
                return;

            DrawPredefinedPath();
        }

        /// <summary>
        ///     Find the edges of the platform.
        /// </summary>
        [ContextMenu("Find Edges")]
        private void FindEdges()
        {
            _positionAOffset = FindEdgeInDirection(true, 0.1f);
            _positionBOffset = FindEdgeInDirection(false, 0.1f);
        }

        /// <summary>
        ///     Find the edge in the specified direction.
        /// </summary>
        /// <param name="isLeft">Is the direction left?</param>
        /// <param name="stepSize">Size of each step.</param>
        /// <returns>Distance to the edge or max distance if edge was not found.</returns>
        private float FindEdgeInDirection(bool isLeft, float stepSize)
        {
            var startingPosition = _localTransform.position;
            var distance = 0f;
            
            while (distance < _maxDistance)
            {
                var position = startingPosition;
                position.x += isLeft ? -distance : distance;
                var hit = Physics2D.Raycast(position, Vector2.down, 1f);
                if (hit.collider == null)
                {
                    Debug.DrawRay(position, Vector2.down, Color.green, 3f);    
                    break;
                }

                Debug.DrawRay(position, Vector2.down, Color.red, 3f);
                distance += stepSize;
            }
            
            distance -= _enemyWidth / 2;
            return distance * (isLeft ? -1 : 1);
        }

        private void DrawPredefinedPath()
        {
            _localTransform = transform;
            var position = _localTransform.position;
            
            Gizmos.color = Color.red;
            if (_movingToA)
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawSphere(new Vector2(position.x + _positionAOffset, position.y), 0.1f);
            
            Gizmos.color = Color.red;
            if (!_movingToA)
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawSphere(new Vector2(position.x + _positionBOffset, position.y), 0.1f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector2(position.x + _positionAOffset, position.y), new Vector2(position.x + _positionBOffset, position.y));
        }

#endif
    }
}