using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    /// <summary>
    ///     The speed at which the player accelerates.
    /// </summary>
    [SerializeField] 
    private float _accelerationSpeed = 10f;

    [SerializeField] 
    private float _decelerationTime = 0.3f;

    /// <summary>
    ///     The maximum speed at which the player can move.
    /// </summary>
    [SerializeField] 
    private float _maxSpeed = 10f;

    /// <summary>
    ///     The force at which the player jumps.
    /// </summary>
    [SerializeField] 
    private float _jumpingForce = 30f;

    /// <summary>
    ///     The rigidbody2D component of the player.
    /// </summary>
    [SerializeField] 
    private Rigidbody2D _rigidbody2D;

    /// <summary>
    ///     The layer mask of the objects that the player can jump on.
    /// </summary>
    [SerializeField] 
    private LayerMask _jumpingLayerMask;
    
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

    private float _decelerationTimePassed;

    /// <summary>
    ///     We get the player height in the Start method.
    /// </summary>
    private void Start()
    {
        _playerSize = GetComponent<BoxCollider2D>().size;
    }

    /// <summary>
    ///     We handle player movement and jumping
    /// </summary>
    private void Update()
    {
        _movementInput = Input.GetAxisRaw("Horizontal");

        if (!ShouldJump())
            return;
        
        Jump();
    }

    private void FixedUpdate()
    {
        _currentSpeed = _rigidbody2D.velocity.magnitude;
        HorizontalMovement();
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
        if (_currentSpeed >= _maxSpeed)
            return;
        
        _decelerationTimePassed = 0;
        _rigidbody2D.AddForce(direction * _accelerationSpeed, ForceMode2D.Impulse);
    }

    private void Decelerate()
    {
        var velocity = _rigidbody2D.velocity;
        velocity.x = Mathf.Lerp(velocity.x, 0, _decelerationTimePassed / _decelerationTime);
        _decelerationTimePassed += Time.fixedDeltaTime;
                
        _rigidbody2D.velocity = velocity;
    }

    /// <summary>
    ///     We check if the player should jump.
    ///     We need it to check if the player is on the ground and if the player pressed the jump button.
    /// </summary>
    /// <returns>True for jump and false for no jump.</returns>
    private bool ShouldJump() => Input.GetButtonDown("Jump") && CanJump();

    /// <summary>
    ///     Jumping method. We add an impulse force to the player.
    /// </summary>
    private void Jump()
    {
        _rigidbody2D.AddForce(Vector2.up * _jumpingForce, ForceMode2D.Impulse);
    }

    /// <summary>
    ///     We check if the player can jump from current position.
    ///     We do it by casting two rays from the player's left and right side.
    /// </summary>
    /// <returns>True for valid position, and false for invalid.</returns>
    private bool CanJump()
    {
        var raycastLeft = Physics2D.Raycast(_rigidbody2D.position - Vector2.left * _playerSize.x / 2, 
            Vector2.down, _playerSize.y / 2 + 0.01f, _jumpingLayerMask);
        var raycastRight = Physics2D.Raycast(_rigidbody2D.position + Vector2.left * _playerSize.x / 2, 
            Vector2.down, _playerSize.y / 2 + 0.01f, _jumpingLayerMask);
        return raycastLeft.collider != null || raycastRight.collider != null;
    }
}
