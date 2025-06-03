using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemies = 3;
    [SerializeField] private float homeRange = 10f;

    public List<GameObject> enemyPool = new List<GameObject>();

    void Start()
    {
        InitializeEnemyPool();
        SpawnEnemy();
    }

    private void InitializeEnemyPool()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    public void SpawnEnemy()
    {
        foreach (var enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.transform.position = GetRandomPositionWithinRange();
                enemy.SetActive(true);

                EnemyBehaviour enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
                if (enemyBehaviour != null)
                {
                    enemyBehaviour.SetHomeRange(homeRange);
                }
            }
        }
    }

    private Vector2 GetRandomPositionWithinRange()
    {
        Vector2 randomOffset = Random.insideUnitCircle * homeRange;
        return (Vector2)transform.position + randomOffset;
    }

    public void DeactivateEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    public void DeactivateAllEnemies()
    {
        foreach (var enemy in enemyPool)
        {
            enemy.SetActive(false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, homeRange);
    }
}