using System;
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
        [SerializeField] private float _maxFollowSpeed = 5f;

        /// <summary>
        /// Minimum camera Y position.
        /// </summary>
        [SerializeField] private float _minimumY;

        /// <summary>
        ///     Camera movement curve.
        /// </summary>
        [SerializeField] private AnimationCurve _movementCurve = AnimationCurve.Linear(0,0,1,1);

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

        private void Awake()
        {
            _cameraTransform = _mainCamera.transform;
        }

        private void Update()
        {
            if (!_isFollowing)
                return;
            
            var targetPosition = _followTarget.position;
            var cameraPosition = _cameraTransform.position;
            targetPosition.z = cameraPosition.z;
            targetPosition.y = Mathf.Max(targetPosition.y, _minimumY);
            
            var movementSpeed = GetMovementPosition(targetPosition, cameraPosition);
            _cameraTransform.position = Vector3.Lerp(cameraPosition, targetPosition, movementSpeed);
        }

        /// <summary>
        ///     Sets new camera follow target.
        /// </summary>
        /// <param name="target">New target to follow.</param>
        public void FollowObject(Transform target)
        {
            _followTarget = target;
            _isFollowing = true;
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
        private float GetMovementPosition(Vector3 targetPosition, Vector3 cameraPosition)
        {
            var distance = Vector2.Distance(targetPosition, cameraPosition);
            return _movementCurve.Evaluate(distance) * _maxFollowSpeed * Time.deltaTime;
        }
    }
}