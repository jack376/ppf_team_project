using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public static List<Enemy> currentEnemies = new List<Enemy>();
    
    public Transform[] spawnPoints;

    public Enemy bossPrefab;
    public List<Enemy> enemyPrefabs = new List<Enemy>();

    public float spawnRadius = 4f;
    public float spawnInterval = 0.1f;
    public float spawnCooldown = 1f;
    public float bigWaveCooldown = 180f;
    public float bigWaveDuration = 60f;
    public int maxEnemies = 200;

    public float powerUpInterval = 60f;
    public float powerUpScaleFactor = 0.1f;

    public float bossSpawnCooldown = 300f;
    public float lastBossSpawnTime = 0f;

    private float nextBigWaveTime = 0f;
    private bool isBigWave = false;
    private float corpseDestroyTime = 1f;
    private float lastPowerUpTime;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (Time.time - lastPowerUpTime > powerUpInterval)
        {
            powerUpScaleFactor += 0.1f;
            lastPowerUpTime = Time.time;
        }

        if (Time.time - lastBossSpawnTime > bossSpawnCooldown)
        {
            CreateBoss();
            lastBossSpawnTime = Time.time;
        }
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
        nextBigWaveTime = Time.time + bigWaveCooldown;

        while (true)
        {
            if (Time.time >= nextBigWaveTime)
            {
                isBigWave = true;
                nextBigWaveTime = Time.time + bigWaveCooldown;
            }

            if (currentEnemies.Count <= maxEnemies)
            {
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    CreateEnemies(spawnPoints[i]);
                    yield return new WaitForSeconds(spawnInterval);
                }
            }

            if (isBigWave)
            {
                yield return new WaitForSeconds(bigWaveDuration);
                isBigWave = false;
            }
            else
            {
                yield return new WaitForSeconds(spawnCooldown);
            }
        }
    }

    private void CreateEnemies(Transform spawnPoint)
    {
        int enemiesToSpawn = isBigWave ? maxEnemies - currentEnemies.Count : 1;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 randomPosition = spawnPoint.position + Random.insideUnitSphere * spawnRadius;
            Enemy enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], randomPosition, Quaternion.identity);

            enemy.enemyData.maxHealth    *= 1 + powerUpScaleFactor;
            enemy.enemyData.attackDamage *= 1 + powerUpScaleFactor;
            enemy.enemyData.attackSpeed  *= 1 + powerUpScaleFactor;

            currentEnemies.Add(enemy);
            enemy.onDeath += () =>
            {
                currentEnemies.Remove(enemy);
                Destroy(enemy.gameObject, corpseDestroyTime);
            };
        }
    }

    void CreateBoss()
    {
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Enemy boss = Instantiate(bossPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
        currentEnemies.Add(boss);

        boss.onDeath += () =>
        {
            currentEnemies.Remove(boss);
            Destroy(boss.gameObject, corpseDestroyTime);
        };
    }
}