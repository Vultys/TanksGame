using System;
using UnityEngine;

public class DestroySelfOnCollisionWithEnemy : MonoBehaviour
{
    public event Action OnHide;

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            OnHide?.Invoke();
        }    
    }
}
