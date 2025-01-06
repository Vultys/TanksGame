using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ProjectileLauncher _gun;

    public event Action<Enemy> OnCollidedWithEnemy;

    public void Init(ProjectileLauncher gun)
    {
        _gun = gun;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        _gun.ReleaseBulletFromPool(this);

        if(other.CompareTag("Enemy"))
        {
            Enemy collidedEnemy = other.GetComponentInParent<Enemy>();
            if(collidedEnemy != null)
            {
                collidedEnemy.ReleaseFromPool();
            }
        }
    }
}
