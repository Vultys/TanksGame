using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// Handles player movement and rotation.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour, IJsonSaveable
{
    [Header("References")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _bodyTransform;

    [Header("Settings")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _turningRate = 180f;

    private Rigidbody2D _rigidbody;
    private Vector2 _previousMovementInput;
    private IMovementHandler _movement;

    private void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();    
        _movement = new PlayerStandardMovementHandler(_rigidbody, _bodyTransform, _movementSpeed, _turningRate);
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

    /// <summary>
    /// Captures the player's position as a JToken.
    /// </summary>
    /// <returns>A JToken representing the player's position.</returns>
    public JToken CaptureAsJToken()
    {
        return transform.position.ToToken();
    }

    /// <summary>
    /// Restores the player's position from the provided JToken.
    /// </summary>
    /// <param name="state">A JToken representing the player's position.</param>
    public void RestoreFromJToken(JToken state)
    {
        transform.position = state.ToVector3();
    }
}
