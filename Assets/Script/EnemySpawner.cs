using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public Transform[] spawnPoints;

    private List<Enemy> enemies = new List<Enemy>();
    private int wave;

    private void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        if (enemies.Count <= 0)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        wave++;
        int count = Mathf.RoundToInt(wave * 1.5f);
        for (int i = 0; i < count; i++)
        {
            float enemyIntensity = Random.Range(0f, 1f);
            CreateEnemy(enemyIntensity);
        }
    }

    private void CreateEnemy(float intensity)
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Enemy enemy = Instantiate(enemyPrefab, point.position, point.rotation);
        
        enemies.Add(enemy);
        enemy.onDeath += () =>
        {
            enemies.Remove(enemy);
            Destroy(enemy.gameObject, 5f);
            //GameManager.instance.AddScore(100);
        };
    }
}