using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private Transform _turretTransform;

    [SerializeField] private InputReader _inputReader;

    private void LateUpdate() 
    {
        Vector2 _cursorPosition = Camera.main.ScreenToWorldPoint(_inputReader.AimPosition);

        _turretTransform.up = _cursorPosition - (Vector2) _turretTransform.position;    
    }
}
