using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomPlatformer.Enemies
{
    /// <summary>
    ///     This class is responsible for the flying enemies. They fly on a predefined path.
    /// </summary>
    public class FlyingEnemy : MonoBehaviour
    {
        /// <summary>
        ///     Enemy movement animator.
        /// </summary>
        [SerializeField] private Animator _movementAnimator;
        
        /// <summary>
        ///     Enemy movement speed.
        /// </summary>
        [SerializeField] private float _speed = 1;

        /// <summary>
        ///     Path points.
        /// </summary>
        [SerializeField] private List<Vector2> _positions;
        
        /// <summary>
        ///     Movement direction.
        /// </summary>
        [SerializeField] private int _direction = 1;

        /// <summary>
        ///     Size of the wave while moving.
        /// </summary>
        [SerializeField] private float _waveSize = 0.2f;

        /// <summary>
        ///     Speed of the waving motion.
        /// </summary>
        [SerializeField] private float _waveSpeed = 1;

        /// <summary>
        ///     Enemy visual.
        /// </summary>
        [SerializeField] private Transform _visual;

#if UNITY_EDITOR
        [SerializeField] private bool _drawGizmos;
#endif

        /// <summary>
        ///     Is enemy moving?
        /// </summary>
        private bool _isMoving;

        /// <summary>
        ///     Enemy local transform.
        /// </summary>
        private Transform _localTransform;
        
        /// <summary>
        ///     Current position index.
        /// </summary>
        private int _currentPositionIndex;
        
        /// <summary>
        ///     Target position index.
        /// </summary>
        private int _targetPositionIndex;

        /// <summary>
        ///     Target position.
        /// </summary>
        private Vector2 _targetPosition;
        
        /// <summary>
        ///     Current wave.
        /// </summary>
        public float _currentWave;
        
        /// <summary>
        ///     Target wave size.
        /// </summary>
        private float _targetWave;

        /// <summary>
        ///     Is enemy facing left?
        /// </summary>
        private bool _facingLeft;

        /// <summary>
        ///     Moving left bool hash.
        ///     We use it to trigger correct animation.
        /// </summary>
        private static readonly int MovingLeft = Animator.StringToHash("MovingLeft");

        /// <summary>
        ///     Assign local transform.
        /// </summary>
        private void Awake()
        {
            _localTransform = transform;
        }

        /// <summary>
        ///     Enable movement.
        /// </summary>
        private void OnEnable()
        {
            _isMoving = true;
            _targetPosition = _positions[0];
        }

        /// <summary>
        ///     Stop movement.
        /// </summary>
        private void OnDisable()
        {
            _isMoving = false;
            _localTransform.position = _positions[0];
            _targetPosition = _positions[0];
            _currentPositionIndex = 0;
            _targetPositionIndex = 0;
            _currentWave = 0;
        }

        private void Update()
        {
            if (!_isMoving)
                return;

            if (_waveSize > 0)
            {
                UpdateWave();
            }

            var position = _localTransform.position;
            if (Vector2.Distance(position, _targetPosition) < 0.1f)
            {
                _currentPositionIndex = _targetPositionIndex;
                _targetPosition = GetNextPosition();
            }
            
            _localTransform.position = Vector2.MoveTowards(position, _targetPosition, _speed * Time.deltaTime);
            _visual.position = _localTransform.position + _currentWave * Vector3.up;
            _facingLeft = position.x > _targetPosition.x;
            _movementAnimator.SetBool(MovingLeft, _facingLeft);
        }

        /// <summary>
        ///     Get the next position in the path.
        /// </summary>
        /// <returns>Next position.</returns>
        private Vector2 GetNextPosition()
        {
            var nextPositionIndex = _currentPositionIndex + _direction;
            
            if (nextPositionIndex < 0)
                nextPositionIndex = _positions.Count - 1;
            
            if (nextPositionIndex >= _positions.Count)
                nextPositionIndex = 0;
            
            _targetPositionIndex = nextPositionIndex;
            return _positions[nextPositionIndex];
        }

        /// <summary>
        ///     Update wave offset.
        /// </summary>
        private void UpdateWave()
        {
            _currentWave = Mathf.Sin(Time.time * _waveSpeed) * _waveSize;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_drawGizmos || _positions == null || _positions.Count <= 0)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.1f);

            Gizmos.color = Color.green;
            for (var i = 0; i < _positions.Count; i++)
            {
                var position = _positions[i];
                Gizmos.DrawSphere(position, 0.1f);

                if (i <= 0) 
                    continue;
                var previousPosition = _positions[i - 1];
                Gizmos.DrawLine(previousPosition, position);
            }
            
            var lastPosition = _positions.Last();
            Gizmos.DrawLine(lastPosition, _positions[0]);
        }
#endif
    }
}