using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Handles collision logic and post-collision behavior.
/// </summary>
public class CollisionHandler : ICollisionHandler
{
    private readonly Rigidbody2D _rigidbody;
    private readonly float _rotationSpeed;
    private readonly float _minAngle;
    private readonly float _maxAngle;
    private bool _isCollided;       
    private Coroutine _currentResetCoroutine;

    public bool IsCollided => _isCollided;

    /// <summary>
    /// Constructor for CollisionHandler.
    /// </summary>
    /// <param name="rigidbody">Rigidbody2D component.</param>
    /// <param name="rotationSpeed">Speed of rotation after collision.</param>
    /// <param name="minAngle">Minimum angle of direction change after collision.</param>
    /// <param name="maxAngle">Maximum angle of direction change after collision.</param>
    public CollisionHandler(Rigidbody2D rigidbody, float rotationSpeed, float minAngle, float maxAngle)
    {
        _rigidbody = rigidbody;
        _rotationSpeed = rotationSpeed;
        _minAngle = minAngle;
        _maxAngle = maxAngle;
    }

    /// <summary>
    /// Handles collision by resetting velocity and initiating post-collision rotation.
    /// </summary>
    /// <param name="collision">Collision data.</param>
    public void HandleCollision(Collision2D collision)
    {
        _rigidbody.velocity = Vector2.zero;
        _isCollided = true;
        
        if (_currentResetCoroutine != null)
        {
            TimerBehaviour.Instance.StopCoroutine(_currentResetCoroutine);
        }

        _currentResetCoroutine = TimerBehaviour.Instance.StartCoroutine(ResetCollisionStatus(RandomRotation()));
    }

    /// <summary>
    /// Applies a random rotation to the object and calculates the time required for the rotation to complete based on the rotation speed.
    /// </summary>
    private float RandomRotation()
    {
        float angleChange = UnityEngine.Random.Range(_minAngle, _maxAngle);

        if(angleChange < 0f)
        {
            angleChange = Mathf.Min(angleChange, -90f);
        }
        else
        {
            angleChange = Mathf.Max(angleChange, 90f);
        }

        Quaternion rotation = Quaternion.AngleAxis(angleChange, Vector3.forward);
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotation * _rigidbody.transform.up);
        Quaternion newRotation = Quaternion.RotateTowards(_rigidbody.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _rigidbody.SetRotation(newRotation);
        return Mathf.Abs(angleChange / _rotationSpeed);
    }
    
    /// <summary>
    /// Coroutine to reset collision status after a delay.
    /// </summary>
    /// <param name="delay">Delay in seconds.</param>
    private IEnumerator ResetCollisionStatus(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isCollided = false;
        _currentResetCoroutine = null;
    }
}
