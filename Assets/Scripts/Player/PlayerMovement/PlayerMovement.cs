using UnityEngine;

/// <summary>
/// Handles player movement and rotation.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _bodyTransform;

    [Header("Settings")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _turningRate = 180f;

    private Rigidbody2D _rigidbody;
    private Vector2 _previousMovementInput;
    private IMovement _movement;

    private void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();    
        _movement = new PlayerStandardMovement(_rigidbody, _bodyTransform, _movementSpeed, _turningRate);
    }

    private void OnEnable() 
    {
        _inputReader.MoveEvent += OnMoveInputReceived;    
    }

    private void OnDisable() 
    {
        _inputReader.MoveEvent -= OnMoveInputReceived;    
    }

    private void Update() 
    {
        _movement.Rotate(_previousMovementInput);
    }

    private void FixedUpdate() 
    {
        _movement.Move(_previousMovementInput);
    }

    /// <summary>
    /// Receives movement input from the input system and stores it for later use.
    /// </summary>
    /// <param name="movementInput">The input vector representing movement directions.</param>
    private void OnMoveInputReceived(Vector2 movementInput)
    {
        _previousMovementInput = movementInput;
    }
}
