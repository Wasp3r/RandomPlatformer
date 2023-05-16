using System.Collections;
using UnityEngine;

namespace RandomPlatformer.Enemies
{
    /// <summary>
    ///     Linear movement for enemies and potentially other objects.
    ///     It's responsible for moving the object from point A to point B and back with given speed.
    /// </summary>
    public class LinearMovement : MonoBehaviour
    {
        /// <summary>
        ///     Offset to the target position.
        /// </summary>
        [SerializeField] private Vector2 _targetOffset;

        /// <summary>
        ///     Duration of one way movement.
        /// </summary>
        [SerializeField] private float _movementDuration = 2f;

        /// <summary>
        ///     Duration of the stop between movements.
        /// </summary>
        [SerializeField] private float _stopDuration = 1f;

        /// <summary>
        ///     Delay before the movement starts.
        /// </summary>
        [SerializeField] private float _delayBeforeStart = 0f;

        /// <summary>
        ///     Is the object meant to come back to the starting position?
        /// </summary>
        [SerializeField] private bool _comeBack = true;

#if UNITY_EDITOR
        [SerializeField] private bool _showGizmos = true;
#endif
        
        /// <summary>
        ///     Movement coroutine.
        /// </summary>
        private Coroutine _movementCoroutine;
        
        /// <summary>
        ///     Is the object currently moving?
        /// </summary>
        private bool _isMoving;
        
        /// <summary>
        ///     Is the object currently going back to the starting position?
        /// </summary>
        private bool _isGoingBack;
        
        /// <summary>
        ///     Starting position of the object.
        /// </summary>
        private Vector2 _startingPosition;
        
        /// <summary>
        ///     Target position of the object.
        /// </summary>
        private Vector2 _targetPosition;
        
        /// <summary>
        ///     Local transform of the object.
        /// </summary>
        private Transform _localTransform;

        /// <summary>
        ///     Starts the movement coroutine.
        /// </summary>
        private void OnEnable()
        {
            _localTransform = transform;
            _startingPosition = _localTransform.position;
            _targetPosition = _startingPosition + _targetOffset;
            StartMovement();
        }
        
        /// <summary>
        ///     Stops the movement coroutine.
        /// </summary>
        private void OnDisable()
        {
            _isMoving = false;
            if (_movementCoroutine != null)
            {
                StopCoroutine(_movementCoroutine);
            }
        }

        /// <summary>
        ///     Starts the movement.
        /// </summary>
        public void StartMovement()
        {
            if (_movementCoroutine != null)
            {
                StopCoroutine(_movementCoroutine);
            }
            
            _isMoving = true;
            _movementCoroutine = StartCoroutine(MovementCoroutine());
        }

        /// <summary>
        ///     Coroutine responsible for moving the object.
        /// </summary>
        private IEnumerator MovementCoroutine()
        {
            yield return new WaitForSeconds(_delayBeforeStart);
            
            while (_isMoving)
            {
                var targetPosition = _isGoingBack ? _startingPosition : _targetPosition;
                var startPosition = _isGoingBack ? _targetPosition : _startingPosition;
                var timePassed = 0f;
                
                while (timePassed < _movementDuration)
                {
                    timePassed += Time.deltaTime;
                    _localTransform.position = Vector2.Lerp(startPosition, targetPosition, timePassed / _movementDuration);
                    yield return null;
                }

                if (!_comeBack)
                    yield break;
                
                yield return new WaitForSeconds(_stopDuration);
                _isGoingBack = !_isGoingBack;
            }
            
            _isMoving = false;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (!_showGizmos)
                return;

            if (Application.isPlaying)
                return;
            
            _startingPosition = transform.position;
            _targetPosition = _startingPosition + _targetOffset;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_startingPosition, _targetPosition);
        }

#endif
    }
}