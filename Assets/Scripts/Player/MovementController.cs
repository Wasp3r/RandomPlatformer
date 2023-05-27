using System;
using System.Collections;
using System.Collections.Generic;
using RandomPlatformer.MainSceneMachine;
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
        ///     The speed at which the jumping force decays.
        /// </summary>
        [SerializeField] private float _jumpingForceDecaySpeed = 0.5f;

        /// <summary>
        ///     The curve that controls the jumping force decay.
        /// </summary>
        [SerializeField] private AnimationCurve _jumpingForceCurve = AnimationCurve.Linear(0, 1, 1, 0);

        /// <summary>
        ///     The maximum number of jumps the player can make in air.
        /// </summary>
        [SerializeField] private int _maxJumpCount = 1;

        /// <summary>
        ///     The rigidbody2D component of the player.
        /// </summary>
        [SerializeField] private Rigidbody2D _rigidbody2D;
        
        /// <summary>
        ///     The height of the player.
        /// </summary>
        [SerializeField]
        private Vector2 _playerSize;

        /// <summary>
        ///     The layer mask of the objects that the player can jump on.
        /// </summary>
        [SerializeField] private LayerMask _solidMask;

        /// <summary>
        ///     The input system actions.
        /// </summary>
        private DefaultInputActions _actions;
        
        /// <summary>
        ///     The lives controller of the player.
        ///     We need it to listen to player losing lives.
        /// </summary>
        private LivesController _livesController;

        /// <summary>
        ///     The current speed of the player.
        /// </summary>
        private float _currentSpeed;

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
        ///     The time passed since the player started jumping.
        /// </summary>
        private float _jumpPowerTimePassed;

        /// <summary>
        ///     The time passed since the player started decelerating.
        /// </summary>
        private float _decelerationTimePassed;

        /// <summary>
        ///     Indicates if the player is touching the ground.
        /// </summary>
        private bool _isGrounded;

        /// <summary>
        ///     Indicates if the player is in the middle of a jump.
        /// </summary>
        private bool _isInJump;

        /// <summary>
        ///     Current jump power.
        ///     We use it to control the height of the jump and to make the force smaller with time.
        /// </summary>
        private float _jumpPower;
        
        /// <summary>
        ///     The initial position of the player.
        /// </summary>
        private Vector3 _checkpointPosition;

        /// <summary>
        ///     The coroutine that handles the grounded state.
        /// </summary>
        private Coroutine _groundedCoroutine;

        /// <summary>
        ///     We get the player height in the Start method.
        /// </summary>
        private void Awake()
        {
            _livesController = GameStateMachine.Instance.LivesController;
            _actions = GameStateMachine.Instance.InputActions;
            _actions.Player.Jump.started += OnJumpStart;
            _actions.Player.Jump.canceled += OnJumpStop;
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
            UpdateJump();
        }

        /// <summary>
        ///     We enable the input system actions.
        /// </summary>
        private void OnEnable()
        {
            _actions.Player.Enable();
            _checkpointPosition = transform.position;
            GameStateMachine.Instance.CameraController.FollowObject(transform);
            _livesController.OnLostLive += ResetPosition;
        }

        /// <summary>
        ///     We disable the input system actions.
        /// </summary>
        private void OnDisable()
        {
            _actions.Player.Disable();
            _actions.Player.Jump.performed -= OnJumpStart;
            _livesController.OnLostLive -= ResetPosition;
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

            _isInJump = false;
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
        private void OnJumpStart(InputAction.CallbackContext context)
        {
            Debug.Log("### - Jump input triggered");
            if (!CanJump())
                 return;

            Debug.Log("### - Starting jump");
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
            _jumpPowerTimePassed = 0;
            _jumpPower = _jumpingForce;
            _isInJump = true;
            _jumpCount++;
            
            if (_groundedCoroutine != null)
                StopCoroutine(_groundedCoroutine);
            _groundedCoroutine = StartCoroutine(GroundedCoroutine());
        }
        
        /// <summary>
        ///     Stops the jumping process.
        ///     We use it to stop adding force when the player releases the jump button.
        /// </summary>
        /// <param name="context">Input system context.</param>
        private void OnJumpStop(InputAction.CallbackContext context)
        {
            Debug.Log("### - Jump input stopped");
            _isInJump = false;
        }

        /// <summary>
        ///     Here is the main jumping method. We add force and decay it over time using <see cref="_jumpingForceCurve"/>
        ///     We use it to have smooth jumping and falling. This also gives us the ability to do small jumps.
        /// </summary>
        private void UpdateJump()
        {
            if (!_isInJump)
                return;

            _jumpPower = _jumpingForceCurve.Evaluate(_jumpPowerTimePassed) * _jumpingForce;
            _jumpPowerTimePassed += Time.fixedDeltaTime * _jumpingForceDecaySpeed;
            
            if (_jumpPower <= 0 || _jumpPowerTimePassed >= 1)
                _isInJump = false;

            Debug.Log("### - Jumping force: " + _jumpPower);
            _rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }

        /// <summary>
        ///     We check if the player can jump from current position.
        ///     We do it by casting two rays from the player's left and right side.
        /// </summary>
        /// <returns>True for valid position, and false for invalid.</returns>
        private bool CanJump()
        {
            if (_jumpCount < _maxJumpCount && !_isGrounded)
                return true;
            
            if (_jumpCount >= _maxJumpCount)
                return false;

            return true;
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
            Debug.DrawRay(_rigidbody2D.position + Vector2.up * _playerSize.y / 2,
                direction, Color.red);
            var raycastLower = Physics2D.Raycast(_rigidbody2D.position - Vector2.up * _playerSize.y / 2,
                direction, _playerSize.x / 2 + 0.01f, _solidMask);
            Debug.DrawRay(_rigidbody2D.position - Vector2.up * _playerSize.y / 2,
                direction, Color.red);
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
        ///     Reset player position to the last checkpoint.
        /// </summary>
        private void ResetPosition()
        {
            transform.position = _checkpointPosition;
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