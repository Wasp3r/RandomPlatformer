using System;
using System.Collections;
using RandomPlatformer.LevelSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace RandomPlatformer
{
    /// <summary>
    ///     This script is responsible for controlling the camera.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        ///     Main camera reference.
        /// </summary>
        [SerializeField] private Camera _mainCamera;

        /// <summary>
        ///     Camera follow speed.
        /// </summary>
        [SerializeField] private float _followSpeedMultiplier = 5f;

        /// <summary>
        ///     Maximum camera follow speed.
        /// </summary>
        [SerializeField] private float _maxFollowSpeed = 0.1f;

        /// <summary>
        ///     Minimum camera Y position.
        /// </summary>
        [SerializeField] private float _minimumY;

        /// <summary>
        ///     Distance on which camera will clamp to the target position.
        ///     We use it to prevent camera from shaking.
        /// </summary>
        [SerializeField] private float _clampingDistance = 0.01f;

        /// <summary>
        ///     Camera movement curve.
        /// </summary>
        [SerializeField] private AnimationCurve _movementCurve = AnimationCurve.Linear(0,0,1,1);

        /// <summary>
        ///     Should camera movement be snapped?
        ///     When set to true camera will move only by full digits.
        /// </summary>
        [SerializeField] private bool _snapCameraMovement;

        /// <summary>
        ///     Is camera controller initialized?
        /// </summary>
        private bool _isInitialized;
        
        /// <summary>
        ///     Camera transform reference.
        /// </summary>
        private Transform _cameraTransform;
        
        /// <summary>
        ///     Camera follow target.
        /// </summary>
        private Transform _followTarget;
        
        /// <summary>
        ///     Is camera following target?
        /// </summary>
        private bool _isFollowing;
        
        /// <summary>
        ///     Temporary focus coroutine.
        ///     Used to change focus on other objects temporarily.
        /// </summary>
        private Coroutine _temporaryFocusCoroutine;

        /// <summary>
        ///     Cached follow target.
        /// </summary>
        private Transform _cachedFollowTarget;

        /// <summary>
        ///     Add references to components.
        /// </summary>
        private void Start()
        {
            _cameraTransform = _mainCamera.transform;
        }

        /// <summary>
        ///     Update camera position.
        /// </summary>
        private void Update()
        {
            if (!_isFollowing || _followTarget.IsDestroyed())
                return;

            var targetPosition = GetFollowingPosition();
            if (_snapCameraMovement)
            {
                targetPosition.x = Mathf.RoundToInt(targetPosition.x);
                targetPosition.y = Mathf.RoundToInt(targetPosition.y);
            }
            _cameraTransform.position = targetPosition;
        }

        /// <summary>
        ///     Sets new camera follow target.
        /// </summary>
        /// <param name="target">New target to follow.</param>
        public void FollowObject(Transform target)
        {
            _followTarget = target;
            _isFollowing = true;
            TeleportToTarget();
        }

        /// <summary>
        ///     Sets new camera follow target position without lerping.
        /// </summary>
        public void TeleportToTarget()
        {
            var position = GetFollowingPosition(true);
            _cameraTransform.position = position;
        }
        
        /// <summary>
        ///     Changes camera focus temporarily and then returns to previous target.
        ///     Used to focus on other objects temporarily to show them.
        /// </summary>
        /// <param name="target">New target to focus on.</param>
        /// <param name="duration">Duration of the focus in seconds.</param>
        public void ChangeFocusTemporarily(Transform target, float duration)
        {
            if (_temporaryFocusCoroutine != null)
                StopCoroutine(_temporaryFocusCoroutine);
            
            _temporaryFocusCoroutine = StartCoroutine(TemporaryFocus(target, duration));
        }
        
        /// <summary>
        ///     Stops following current target.
        /// </summary>
        public void StopFollowing()
        {
            _isFollowing = false;
            _followTarget = null;
        }

        /// <summary>
        ///     Get camera movement speed based on the distance between target and camera. We use animation curve to get the
        ///     movement speed.
        /// </summary>
        /// <param name="targetPosition">Target position.</param>
        /// <param name="cameraPosition">Camera position.</param>
        /// <returns></returns>
        private float GetMovementSpeed(Vector3 targetPosition, Vector3 cameraPosition)
        {
            var distance = Vector2.Distance(targetPosition, cameraPosition);
            if (distance < _clampingDistance)
                return 1f;
            
            return Mathf.Clamp01(_movementCurve.Evaluate(distance) * _followSpeedMultiplier * Time.deltaTime);
        }

        private Vector3 GetFollowingPosition(bool teleport = false)
        {
            var targetPosition = _followTarget.position;
            var cameraPosition = _cameraTransform.position;
            targetPosition.z = cameraPosition.z;
            targetPosition.y = Mathf.Max(targetPosition.y, _minimumY);
            
            var movementProgress = GetMovementSpeed(targetPosition, cameraPosition);
            movementProgress = Mathf.Min(movementProgress, _maxFollowSpeed);
            if (teleport)
            {
                movementProgress = 1;
            }
            
            return Vector3.Lerp(cameraPosition, targetPosition, movementProgress);
        }
        
        /// <summary>
        ///     Temporary focus coroutine.
        ///     Used to change focus on other objects temporarily and then return to previous target.
        /// </summary>
        /// <param name="target">New target to focus on.</param>
        /// <param name="duration">Duration of the focus in seconds.</param>
        private IEnumerator TemporaryFocus(Transform target, float duration)
        {
            if (!_cachedFollowTarget)
            {
                _cachedFollowTarget = _followTarget;
            }
            
            _followTarget = target;
            yield return new WaitForSeconds(duration);
            _followTarget = _cachedFollowTarget;
            _cachedFollowTarget = null;
        }
    }
}