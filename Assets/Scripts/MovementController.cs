using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    /// <summary>
    ///     The speed at which the player accelerates.
    /// </summary>
    [SerializeField] 
    private float _accelerationSpeed = 1f;

    /// <summary>
    ///     The speed at which the player decelerates.
    /// </summary>
    [SerializeField] 
    private float _decelerationSpeed = 2f;
    
    /// <summary>
    ///     The maximum speed at which the player can move.
    /// </summary>
    [SerializeField] 
    private float _maxSpeed = 1f;

    /// <summary>
    ///     The rigidbody2D component of the player.
    /// </summary>
    [SerializeField] 
    private Rigidbody2D _rigidbody2D;
    
    /// <summary>
    ///     The current speed of the player.
    /// </summary>
    private float _currentSpeed;
    
    /// <summary>
    ///     The previous position of the player.
    /// </summary>
    private Vector2 _previousPosition;

    /// <summary>
    ///     We handle player movement in the Update method.
    /// </summary>
    void FixedUpdate()
    {
        var position = _rigidbody2D.position;
        var direction = position - _previousPosition;
        _currentSpeed = _rigidbody2D.velocity.magnitude;
        _previousPosition = position;
        
        if (_currentSpeed >= _maxSpeed)
            return;
        
        switch (Input.GetAxisRaw("Horizontal"))
        {
            case 1:
                _rigidbody2D.AddForce(Vector2.right * _accelerationSpeed);
                break;
            case -1:
                _rigidbody2D.AddForce(Vector2.left * _accelerationSpeed);
                break;
            default:
                direction.y = 0;
                _rigidbody2D.AddForce(-direction * _decelerationSpeed);
                break;
        }
    }
}
