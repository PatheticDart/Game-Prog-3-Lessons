using UnityEngine;

public class EnemyFactoryInput : MonoBehaviour
{
    [SerializeField] private EnemyFactory enemyFactory;
    [SerializeField] private Transform spawnPoint;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            enemyFactory.CreateEnemy(EnemyType.Giant, spawnPoint.position, spawnPoint.rotation);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            enemyFactory.CreateEnemy(EnemyType.Crawler, spawnPoint.position, spawnPoint.rotation);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            enemyFactory.CreateEnemy(EnemyType.Mutant, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
