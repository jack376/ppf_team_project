using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static List<Enemy> currentEnemies = new List<Enemy>();
    // 게임 시작부터 끝까지만 유지하도록 이따가 수정하기

    public List<Enemy> enemyGroup = new List<Enemy>();
    public Transform[] spawnPoints;
    public float spawnRadius = 4f;

    private int wave;

    private void Start()
    {
        StartCoroutine(SpawnWaveCoroutine());
    }

    private IEnumerator SpawnWaveCoroutine()
    {
        while (true)
        {
            //yield return new WaitUntil(() => currentEnemies.Count <= 0); 
            // 맵에 몬스터가 없을 때

            wave++;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                CreateEnemies(spawnPoints[i]);
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(60f); // 60초가 지났을 경우
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
                Destroy(enemy.gameObject, 1f);
            };
        }
    }
}