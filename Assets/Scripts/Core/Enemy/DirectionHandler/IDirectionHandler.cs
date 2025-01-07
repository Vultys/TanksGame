/// <summary>
/// Interface for managing direction updates and rotation.
/// </summary>
public interface IDirectionHandler
{
    void UpdateDirection(float deltaTime);
    void RotateTowardsTarget();
}
