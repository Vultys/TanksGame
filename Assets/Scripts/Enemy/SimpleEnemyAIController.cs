using UnityEngine;

public class SimpleEnemyAIController : MonoBehaviour
{
    [SerializeField] private float _minDirectionChangeCooldown = 1f;

    [SerializeField] private float _maxDirectionChangeCooldown = 4f;

    [SerializeField] private float _minDirectionChangeAngle = -90f;

    [SerializeField] private float _maxDirectionChangeAngle = 90f;

    [SerializeField] private float _speed = 3f;

    [SerializeField] private float _rotationSpeed = 90f;

    [SerializeField] private Rigidbody2D _rigidbody;
    
    private float _directionChangeCooldown;

    private Vector2 _targetDirection;

    private void Awake() 
    {
        _targetDirection = transform.up;
        _directionChangeCooldown = Random.Range(_minDirectionChangeCooldown, _maxDirectionChangeCooldown);
    }

    private void FixedUpdate() 
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        _rigidbody.velocity = transform.up * _speed;
    }

    public Vector2 UpdateTargetDirection()
    {
        _directionChangeCooldown -= Time.deltaTime;

        if(_directionChangeCooldown <= 0)
        {
            float angleChange = Random.Range(_minDirectionChangeAngle, _maxDirectionChangeAngle);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, _rigidbody.transform.forward);
            _targetDirection = rotation * _targetDirection;

            _directionChangeCooldown = Random.Range(_minDirectionChangeCooldown, _maxDirectionChangeCooldown);
        }

        return _targetDirection;
    }
    
    private void RotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_rigidbody.transform.forward, _targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(_rigidbody.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _rigidbody.SetRotation(rotation);
    }
}