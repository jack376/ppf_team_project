using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 게임 시작부터 끝까지만 유지하도록 나중에 수정하기
    public static List<Enemy> currentEnemies = new List<Enemy>();

    public List<Enemy> enemyGroup = new List<Enemy>();
    public Transform[] spawnPoints;

    public float spawnRadius = 4f;
    public float spawnInterval = 0.5f;
    public float spawnTime = 60f;
    public float corpseDestroyTime = 1f;

    private int wave;

    private void Start()
    {
        StartCoroutine(SpawnWaveCoroutine());
    }

    private IEnumerator SpawnWaveCoroutine()
    {
        yield return new WaitUntil(() => GameManager.isGameover == false);

        while (true)
        {
            //yield return new WaitUntil(() => currentEnemies.Count <= 0); 
            // 맵에 몬스터가 없을 때 

            wave++;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                CreateEnemies(spawnPoints[i]);
                yield return new WaitForSeconds(spawnInterval);
            }
 
            yield return new WaitForSeconds(spawnTime);
        }
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