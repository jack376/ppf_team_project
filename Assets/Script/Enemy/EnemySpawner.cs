using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public static List<Enemy> currentEnemies = new List<Enemy>();
    
    public Transform[] spawnPoints;

    public List<Enemy> bossPrefabs = new List<Enemy>();
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

    public float intervalDecreaseTime = 60f;  // spawnInterval이 감소하는 빈도 (예: 매 60초마다)
    public float cooldownDecreaseTime = 120f; // spawnCooldown이 감소하는 빈도 (예: 매 120초마다)

    public float intervalDecreaseAmount = 0.01f; // spawnInterval이 감소하는 양 (예: 0.01초)
    public float cooldownDecreaseAmount = 0.1f;  // spawnCooldown이 감소하는 양 (예: 0.1초)

    private float lastIntervalDecreaseTime = 0f; // 마지막으로 spawnInterval이 감소한 시간
    private float lastCooldownDecreaseTime = 0f; // 마지막으로 spawnCooldown이 감소한 시간

    private float nextBigWaveTime = 0f;
    private bool isBigWave = false;
    private float corpseDestroyTime = 1f;
    private float lastPowerUpTime;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
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

        if (Time.time - lastIntervalDecreaseTime > intervalDecreaseTime)
        {
            spawnInterval = Mathf.Max(spawnInterval - intervalDecreaseAmount, 0);
            lastIntervalDecreaseTime = Time.time;
        }

        if (Time.time - lastCooldownDecreaseTime > cooldownDecreaseTime)
        {
            spawnCooldown = Mathf.Max(spawnCooldown - cooldownDecreaseAmount, 0);
            lastCooldownDecreaseTime = Time.time;
        }
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "InGameScene")
        {
            lastBossSpawnTime = Time.time;
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

    private void CreateBoss()
    {
        Transform randomSpawnPoint   = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Enemy     selectedBossPrefab = bossPrefabs[Random.Range(0, bossPrefabs.Count)];

        Enemy boss = Instantiate(selectedBossPrefab, randomSpawnPoint.position, Quaternion.identity);
        currentEnemies.Add(boss);

        boss.onDeath += () =>
        {
            currentEnemies.Remove(boss);
            Destroy(boss.gameObject, corpseDestroyTime);
        };
    }
}