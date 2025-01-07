using UnityEngine;

public class Enemy : MonoBehaviour
{
    private TankController _enemyController;

    public void Initialize(TankController controller)
    {
        _enemyController = controller;
    }
    
    public void ReleaseFromPool()
    {
        _enemyController.ReleaseEnemy(this);
    }
}
