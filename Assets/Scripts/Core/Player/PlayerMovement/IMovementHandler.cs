using UnityEngine;

/// <summary>
/// Interface for player movement.
/// </summary>
public interface IMovementHandler
{
    void Move(Vector2 input);

    void Rotate(Vector2 input);
}