using System;
using UnityEngine;

/// <summary>
/// MonoBehaviour for handling collision events with enemies.
/// </summary>
public class PlayerCollisionHandler : MonoBehaviour
{
    public event Action OnCollisionWithEnemy;

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            OnCollisionWithEnemy?.Invoke();
            Destroy(gameObject);
        }    
    }
}
