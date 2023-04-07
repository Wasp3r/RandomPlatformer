using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] 
    private float _accelerationSpeed = 1f;

    [SerializeField] 
    private float _decelerationSpeed = 2f;
    
    [SerializeField] 
    private float _maxSpeed = 1f;
    
    public float _currentSpeed;
    private Vector2 _previousPosition;
    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var position = _rigidbody2D.position;
        var direction = position - _previousPosition;
        _currentSpeed = Vector2.Distance(position, _previousPosition) / Time.deltaTime;
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
                _rigidbody2D.AddForce(-direction * _decelerationSpeed);
                break;
        }
    }
}
