using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace RandomPlatformer.Player
{
    public class MovementController : MonoBehaviour
    {
        /// <summary>
        ///     The speed at which the player accelerates.
        /// </summary>
        [SerializeField] private float _accelerationSpeed = 10f;

        [SerializeField] private float _decelerationTime = 0.3f;

        /// <summary>
        ///     The maximum speed at which the player can move.
        /// </summary>
        [SerializeField] private float _maxSpeed = 10f;

        /// <summary>
        ///     The force at which the player jumps.
        /// </summary>
        [SerializeField] private float _jumpingForce = 30f;

        /// <summary>
        ///     The maximum number of jumps the player can make in air.
        /// </summary>
        [SerializeField] private int _maxJumpCount = 1;

        /// <summary>
        ///     The rigidbody2D component of the player.
        /// </summary>
        [SerializeField] private Rigidbody2D _rigidbody2D;

        /// <summary>
        ///     The layer mask of the objects that the player can jump on.
        /// </summary>
        [SerializeField] private LayerMask _solidMask;

        /// <summary>
        ///     The input system actions.
        /// </summary>
        private DefaultInputActions _actions;

        /// <summary>
        ///     The current speed of the player.
        /// </summary>
        private float _currentSpeed;

        /// <summary>
        ///     The height of the player.
        /// </summary>
        private Vector2 _playerSize;

        /// <summary>
        ///     The movement input of the player.
        /// </summary>
        private float _movementInput;

        /// <summary>
        ///     The jump input of the player.
        /// </summary>
        private bool _shouldJump;

        /// <summary>
        ///     The number of jumps the player has made in air.
        /// </summary>
        private int _jumpCount;

        /// <summary>
        ///     The time passed since the player started decelerating.
        /// </summary>
        private float _decelerationTimePassed;

        /// <summary>
        ///     Indicates if the player is touching the ground.
        /// </summary>
        private bool _isGrounded;

        /// <summary>
        ///     The coroutine that handles the grounded state.
        /// </summary>
        private Coroutine _groundedCoroutine;

        /// <summary>
        ///     We get the player height in the Start method.
        /// </summary>
        private void Awake()
        {
            _actions = GameController.Instance.InputActions;
            _actions.Player.Jump.performed += OnJump;
            
            _playerSize = GetComponent<BoxCollider2D>().size;
            _jumpCount = 0;
        }

        /// <summary>
        ///     We handle player movement and jumping
        /// </summary>
        private void Update()
        {
            _movementInput = _actions.Player.Move.ReadValue<Vector2>().x;
            UpdateIsGrounded();
        }

        /// <summary>
        ///     We handle physics related stuff.
        /// </summary>
        private void FixedUpdate()
        {
            _currentSpeed = _rigidbody2D.velocity.magnitude;
            HorizontalMovement();
        }

        /// <summary>
        ///     We enable the input system actions.
        /// </summary>
        private void OnEnable()
        {
            _actions.Player.Enable();
            GameController.Instance.CameraController.FollowObject(transform);
        }
        
        /// <summary>
        ///     We disable the input system actions.
        /// </summary>
        private void OnDisable()
        {
            _actions.Player.Disable();
            _actions.Player.Jump.performed -= OnJump;
        }

        /// <summary>
        ///     We check if the player is grounded.
        ///     We need it to check if the player can jump or multiple jump.
        /// </summary>
        private void UpdateIsGrounded()
        {
            if (_isGrounded)
                return;

            _isGrounded = IsGrounded();
            if (!_isGrounded)
                return;

            _jumpCount = 0;
        }

        /// <summary>
        ///     We handle horizontal movement.
        /// </summary>
        private void HorizontalMovement()
        {
            switch (_movementInput)
            {
                case 1:
                    Accelerate(Vector2.right);
                    break;
                case -1:
                    Accelerate(Vector2.left);
                    break;
                default:
                    Decelerate();
                    break;
            }
        }

        /// <summary>
        ///     We accelerate the player in the given direction.
        /// </summary>
        /// <param name="direction">Acceleration direction</param>
        private void Accelerate(Vector2 direction)
        {
            // We check if new input is in the same direction as the current velocity.
            // When it is, we shouldn't accelerate more.
            if (_currentSpeed >= _maxSpeed)
            {
                var currentVelocityX = _rigidbody2D.velocity.x;
                var directionX = direction.x;
                if (currentVelocityX > 0 && directionX > 0)
                    return;

                if (currentVelocityX < 0 && directionX < 0)
                    return;
            }

            if (!CanAccelerate(direction))
            {
                _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
                return;
            }

            _decelerationTimePassed = 0;
            _rigidbody2D.AddForce(direction * _accelerationSpeed, ForceMode2D.Impulse);
        }

        /// <summary>
        ///     We decelerate the player.
        ///     We do it by lerping the player's velocity to zero.
        /// </summary>
        private void Decelerate()
        {
            var velocity = _rigidbody2D.velocity;
            velocity.x = Mathf.Lerp(velocity.x, 0, _decelerationTimePassed / _decelerationTime);
            _decelerationTimePassed += Time.fixedDeltaTime;

            _rigidbody2D.velocity = velocity;
        }

        /// <summary>
        ///     Entry point for the jump input.
        ///     We check if the player can jump from current position.
        /// </summary>
        /// <param name="context">Input system context.</param>
        private void OnJump(InputAction.CallbackContext context)
        {
            if (!Input.GetButtonDown("Jump"))
                return;

            if (!CanJump())
                return;

            Jump();
        }

        /// <summary>
        ///     We check if the player can jump from current position.
        ///     We do it by casting two rays from the player's left and right side.
        /// </summary>
        /// <returns>True for valid position, and false for invalid.</returns>
        private bool CanJump()
        {
            if (_jumpCount < _maxJumpCount && !_isGrounded)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
                return true;
            }

            return IsGrounded();
        }

        /// <summary>
        ///     Jumping method. We add an impulse force to the player.
        /// </summary>
        private void Jump()
        {
            _jumpCount++;
            _rigidbody2D.AddForce(Vector2.up * _jumpingForce, ForceMode2D.Impulse);

            if (_groundedCoroutine != null)
                StopCoroutine(_groundedCoroutine);
            _groundedCoroutine = StartCoroutine(GroundedCoroutine());
        }

        /// <summary>
        ///     We check if the player can accelerate in the given direction.
        ///     We do it by casting 2 rays from the player position in the given direction.
        /// </summary>
        /// <param name="direction">Movement direction.</param>
        /// <returns>Can user move in given direction.</returns>
        private bool CanAccelerate(Vector3 direction)
        {
            var raycastUpper = Physics2D.Raycast(_rigidbody2D.position + Vector2.up * _playerSize.y / 2,
                direction, _playerSize.x / 2 + 0.01f, _solidMask);
            var raycastLower = Physics2D.Raycast(_rigidbody2D.position - Vector2.up * _playerSize.y / 2,
                direction, _playerSize.x / 2 + 0.01f, _solidMask);
            return raycastUpper.collider == null && raycastLower.collider == null;
        }

        /// <summary>
        ///     We check if the player is grounded by casting 2 rays from the player's left and right side.
        /// </summary>
        /// <returns>True for grounded, and false for not grounded.</returns>
        private bool IsGrounded()
        {
            var raycastLeft = Physics2D.Raycast(_rigidbody2D.position - Vector2.left * _playerSize.x / 2,
                Vector2.down, _playerSize.y / 2 + 0.01f, _solidMask);
            var raycastRight = Physics2D.Raycast(_rigidbody2D.position + Vector2.left * _playerSize.x / 2,
                Vector2.down, _playerSize.y / 2 + 0.01f, _solidMask);
            return raycastLeft.collider != null || raycastRight.collider != null;
        }

        /// <summary>
        ///     We set the grounded state to false after a short delay.
        ///     This is needed to compensate for few frames of delay after jump is performed.
        /// </summary>
        private IEnumerator GroundedCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            _isGrounded = false;
        }
    }
}