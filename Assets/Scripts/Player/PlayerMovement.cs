using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private Rigidbody2D _rigidbody;

    [Header("Settings")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _turningRate = 180f;

    private Vector2 _previousMovementInput;

    private void OnEnable() 
    {
        _inputReader.MoveEvent += HandleMove;    
    }

    private void OnDisable() 
    {
        _inputReader.MoveEvent -= HandleMove;    
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        Destroy(gameObject);    
    }

    private void Update() 
    {
        Rotate();  
    }

    private void FixedUpdate() 
    {
        Move();
    }

    private void Rotate()
    {
        float zRotation = _previousMovementInput.x * -_turningRate * Time.deltaTime;
        _bodyTransform.Rotate(0f, 0f, zRotation);
    }

    private void Move()
    {
        _rigidbody.velocity = (Vector2) _bodyTransform.up * _previousMovementInput.y * _movementSpeed;
    }

    private void HandleMove(Vector2 movementInput)
    {
        _previousMovementInput = movementInput;
    }
}
