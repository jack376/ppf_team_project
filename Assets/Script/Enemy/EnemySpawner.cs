using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public static List<Enemy> currentEnemies = new List<Enemy>();

    public List<Enemy> enemyGroup = new List<Enemy>();
    public Transform[] spawnPoints;

    public float spawnRadius = 4f;
    public float spawnInterval = 0.5f;
    public float spawnTime = 60f;
    public float corpseDestroyTime = 1f;

    private int wave;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "InGameScene")
        {
            StartCoroutine(SpawnWaveCoroutine());
        }
    }

    private IEnumerator SpawnWaveCoroutine()
    {
        yield return new WaitUntil(() => GameManager.isGameover == false);

        while (true)
        {
            wave++;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                CreateEnemies(spawnPoints[i]);
                yield return new WaitForSeconds(spawnInterval);
            }
 
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void CreateEnemies(Transform spawnPoint)
    {
        foreach (Enemy enemyPrefab in enemyGroup)
        {
            Vector3 randomPosition = spawnPoint.position + Random.insideUnitSphere * spawnRadius;
            Enemy enemy = Instantiate(enemyPrefab, randomPosition, spawnPoint.rotation);

            currentEnemies.Add(enemy);
            enemy.onDeath += () =>
            {
                currentEnemies.Remove(enemy);
                Destroy(enemy.gameObject, corpseDestroyTime);
            };
        }
    }
}