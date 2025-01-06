using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyController _enemyController;

    public void Init(EnemyController controller)
    {
        _enemyController = controller;
    }
    
    public void ReleaseFromPool()
    {
        _enemyController.ReleaseEnemyFromPool(this);
    }
}
