using Newtonsoft.Json.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAIController : MonoBehaviour, IJsonSaveable
{
    [Header("Direction Change Settings")]
    [SerializeField, Range(0f, 10f)] private float _minDirectionChangeCooldown = 1f;
    [SerializeField, Range(1f, 10f)] private float _maxDirectionChangeCooldown = 4f;
    [SerializeField, Range(-360f, 360f)] private float _minDirectionChangeAngle = -90f;
    [SerializeField, Range(-360f, 360f)] private float _maxDirectionChangeAngle = 90f;

    [Header("Movement Settings")]
    [SerializeField] private float _speed = 3f;
    [SerializeField, Range(0, 360f)] private float _rotationSpeed = 90f;

    private Rigidbody2D _rigidbody;
    private ICollisionHandler _collisionHandler;
    private IDirectionHandler _directionHandler;

    private void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collisionHandler = new CollisionHandler(_rigidbody, _rotationSpeed, _minDirectionChangeAngle, _maxDirectionChangeAngle);
        _directionHandler = new DirectionHandler(_rigidbody, _minDirectionChangeCooldown, _maxDirectionChangeCooldown,
                                                            _minDirectionChangeAngle, _maxDirectionChangeAngle, _rotationSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        _collisionHandler.HandleCollision(other);
    }

    private void FixedUpdate() 
    {
        if(_collisionHandler is CollisionHandler handler && handler.IsCollided) return;

        _directionHandler.UpdateDirection(Time.deltaTime);
        _directionHandler.RotateTowardsTarget();
        _rigidbody.velocity = (Vector2)transform.up * _speed;
    }

    /// <summary>
    /// Captures the enemy's position as a JToken.
    /// </summary>
    /// <returns>A JToken representing the enemy's position.</returns>
    public JToken CaptureAsJToken()
    {
        return transform.position.ToToken();
    }

    /// <summary>
    /// Restores the enemy's position from the provided JToken.
    /// </summary>
    /// <param name="state">A JToken representing the enemy's position.</param>
    public void RestoreFromJToken(JToken state)
    {
        transform.position = state.ToVector3();
    }
}