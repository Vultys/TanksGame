using System;
using UnityEngine;

public class DestroySelfOnCollisionWithEnemy : MonoBehaviour
{
    public event Action OnDestroy;

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            OnDestroy?.Invoke();
            Destroy(gameObject);
        }    
    }
}
