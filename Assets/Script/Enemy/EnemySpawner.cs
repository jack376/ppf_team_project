using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static List<Enemy> currentEnemies = new List<Enemy>();
    // ���� ���ۺ��� �������� �����ϵ��� �̵��� �����ϱ�

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
            // �ʿ� ���Ͱ� ���� ��

            wave++;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                CreateEnemies(spawnPoints[i]);
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(60f); // 60�ʰ� ������ ���
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