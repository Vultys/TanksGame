using System.Collections;
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

    private bool _isCollided = false;

    private void Awake() 
    {
        _targetDirection = transform.up;
        _directionChangeCooldown = Random.Range(_minDirectionChangeCooldown, _maxDirectionChangeCooldown);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        _rigidbody.velocity = Vector2.zero;
        _isCollided = true;
        RotateAfterCollision();
    }

    private void FixedUpdate() 
    {
        if(_isCollided)
        {
            return;
        }

        UpdateTargetDirection();
        RotateTowardsTarget();
        _rigidbody.velocity = (Vector2)transform.up * _speed;
    }

    private void RotateAfterCollision()
    {
        float angleChange = Random.Range(_minDirectionChangeAngle, _maxDirectionChangeAngle);
        Quaternion rotation = Quaternion.AngleAxis(angleChange, _rigidbody.transform.forward);
        Quaternion targetRotation = Quaternion.LookRotation(_rigidbody.transform.forward, rotation * transform.up);
        Quaternion newRotation = Quaternion.RotateTowards(_rigidbody.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        _rigidbody.SetRotation(newRotation);

        Invoke("ResetCollisionStatus", angleChange / _rotationSpeed);
    }

    private void ResetCollisionStatus()
    {
        _isCollided = false;
        _directionChangeCooldown = Random.Range(_minDirectionChangeCooldown, _maxDirectionChangeCooldown);
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