using UnityEngine;

public class PlayerStandardMovementHandler : IMovementHandler
{
    private readonly Rigidbody2D _rigidbody;
    private readonly Transform _bodyTransform;
    private readonly float _movementSpeed;
    private readonly float _turningRate;


    /// <summary>
    /// Creates a new instance of the standard player movement.
    /// </summary>
    /// <param name="rigidbody">Rigidbody of the player.</param>
    /// <param name="bodyTransform">Body transform of the player.</param>
    /// <param name="movementSpeed">Speed at which the player moves.</param>
    /// <param name="turningRate">Rate at which the player rotates.</param>
    public PlayerStandardMovementHandler(Rigidbody2D rigidbody, Transform bodyTransform, float movementSpeed, float turningRate)
    {
        _rigidbody = rigidbody;
        _bodyTransform = bodyTransform;
        _movementSpeed = movementSpeed;
        _turningRate = turningRate;
    }

    /// <summary>
    /// Moves the player in the forward direction based on vertical movement input.
    /// </summary>
    /// <param name="input">Input vector representing movement directions.</param>
    public void Move(Vector2 input)
    {
        Vector2 movementDirection = (Vector2) _bodyTransform.up * input.y;
        _rigidbody.velocity = movementDirection * _movementSpeed;
    }

    /// <summary>
    /// Rotates the player body based on horizontal movement input.
    /// </summary>
    public void Rotate(Vector2 input)
    {
        float zRotation = input.x * -_turningRate * Time.deltaTime;
        _bodyTransform.Rotate(0f, 0f, zRotation);
    }
}
