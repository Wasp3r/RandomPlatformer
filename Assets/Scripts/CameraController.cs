using System;
using UnityEngine;

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
        [SerializeField] private float _followSpeed = 5f;

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
            _cameraTransform.position = Vector3.Lerp(cameraPosition, targetPosition, _followSpeed * Time.deltaTime);
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
    }
}