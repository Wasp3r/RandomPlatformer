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

    /// <summary>
    ///     The speed at which the player decelerates.
    /// </summary>
    [SerializeField] 
    private float _decelerationSpeed = 300f;
    
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

    [SerializeField] 
    private LayerMask _jumpingLayerMask;
    
    /// <summary>
    ///     The current speed of the player.
    /// </summary>
    private float _currentSpeed;

    /// <summary>
    ///     The height of the player.
    /// </summary>
    private float _playerHeight;

    /// <summary>
    ///     We get the player height in the Start method.
    /// </summary>
    private void Start()
    {
        _playerHeight = GetComponent<BoxCollider2D>().size.y;
    }

    /// <summary>
    ///     We handle player movement and jumping
    /// </summary>
    private void Update()
    {
        _currentSpeed = _rigidbody2D.velocity.magnitude;
        transform.rotation = Quaternion.identity;
        HorizontalMovement();
        
        if (!ShouldJump())
            return;
        
        Jump();
    }

    private void HorizontalMovement()
    {
        switch (Input.GetAxisRaw("Horizontal"))
        {
            case 1:
                Accelerate(Vector2.right);
                break;
            case -1:
                Accelerate(Vector2.left);
                break;
        }
    }
    
    private void Accelerate(Vector2 direction)
    {
        if (_currentSpeed >= _maxSpeed)
            return;
        
        _rigidbody2D.AddForce(direction * _accelerationSpeed);
    }

    private bool ShouldJump() => Input.GetButtonDown("Jump") && CanJump();

    private void Jump()
    {
        Debug.Log("### - Jumping");
        _rigidbody2D.AddForce(Vector2.up * _jumpingForce, ForceMode2D.Impulse);
    }

    private bool CanJump()
    {
        var raycast = Physics2D.Raycast(_rigidbody2D.position, Vector2.down, _playerHeight + 0.01f, _jumpingLayerMask);
        Debug.Log($"### - Collider: {raycast.collider}");
        return raycast.collider != null;
    }
}
