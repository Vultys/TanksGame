using UnityEngine;

/// <summary>
/// Handles player aiming by rotating the turret toward the cursor's position.
/// </summary>
public class PlayerAiming : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _turretTransform;
    [SerializeField] private InputReader _inputReader;

    private Camera _mainCamera;

    private void Awake() 
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate() 
    {
        UpdateTurretAim();
    }

    /// <summary>
    /// Updates the turret's aim to face the cursor position.
    /// </summary>
    private void UpdateTurretAim()
    {
        if (_mainCamera == null) return;

        Vector2 cursorWorldPosition = _mainCamera.ScreenToWorldPoint(_inputReader.AimPosition);

        Vector2 aimDirection = cursorWorldPosition - (Vector2) _turretTransform.position;
        _turretTransform.up = aimDirection;
    }
}
