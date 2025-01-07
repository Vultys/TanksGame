using UnityEngine;

/// <summary>
/// Manages direction changes and rotation towards targets.
/// </summary>
public class DirectionHandler : IDirectionHandler
{
    private readonly Rigidbody2D _rigidbody;
    private readonly float _minCooldown;
    private readonly float _maxCooldown;
    private readonly float _minAngle;
    private readonly float _maxAngle;
    private readonly float _rotationSpeed;

    private float _cooldown;
    private Vector2 _targetDirection;

    /// <summary>
    /// Constructor for DirectionHandler.
    /// </summary>
    /// <param name="rigidbody">Rigidbody2D component.</param>
    /// <param name="minCooldown">Minimum time between direction changes.</param>
    /// <param name="maxCooldown">Maximum time between direction changes.</param>
    /// <param name="minAngle">Minimum angle of direction change.</param>
    /// <param name="maxAngle">Maximum angle of direction change.</param>
    /// <param name="rotationSpeed">Speed of rotation towards target direction.</param>
    public DirectionHandler(Rigidbody2D rigidbody, float minCooldown, float maxCooldown, float minAngle, float maxAngle, float rotationSpeed)
    {
        _rigidbody = rigidbody;
        _minCooldown = minCooldown;
        _maxCooldown = maxCooldown;
        _minAngle = minAngle;
        _maxAngle = maxAngle;
        _rotationSpeed = rotationSpeed;

        _targetDirection = rigidbody.transform.up;
        ResetCooldown();
    }

    /// <summary>
    /// Updates the target direction based on cooldown timer.
    /// </summary>
    /// <param name="deltaTime">Time elapsed since last update.</param>
    public void UpdateDirection(float deltaTime)
    {
        _cooldown -= deltaTime;

        if (_cooldown > 0) return;

        float angleChange = Random.Range(_minAngle, _maxAngle);
        Quaternion rotation = Quaternion.AngleAxis(angleChange, Vector3.forward);
        _targetDirection = rotation * _targetDirection;

        ResetCooldown();
    }

    /// <summary>
    /// Rotates the object towards the target direction.
    /// </summary>
    public void RotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, _targetDirection);
        Quaternion newRotation = Quaternion.RotateTowards(_rigidbody.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _rigidbody.SetRotation(newRotation);
    }

    /// <summary>
    /// Resets the cooldown timer for direction changes.
    /// </summary>
    private void ResetCooldown()
    {
        _cooldown = Random.Range(_minCooldown, _maxCooldown);
    }
}
