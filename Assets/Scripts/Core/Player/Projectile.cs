using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ProjectileLauncher _gun;

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
            collidedEnemy?.HandleProjectileCollision();
        }
    }
}
