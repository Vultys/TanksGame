using UnityEngine;

/// <summary>
/// Interface for handling collision events.
/// </summary>
public interface ICollisionHandler
{
    void HandleCollision(Collision2D collision);
}
