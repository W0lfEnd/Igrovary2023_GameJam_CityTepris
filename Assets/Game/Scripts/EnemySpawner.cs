using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
struct WaveConfig
{
    public GameObject EnemyPrefab;
    public float EnemiesCount;
    public float WaveDuration;
    public List<GameObject> SpawnedEnemies;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float SpawnRadius;
    [SerializeField] private WaveConfig[] WaveConfig;

    private float WavesTimeProgresss;
    private float StartedAtTimestamp;
    private bool IsSpawning;
    private int LastSpawnedWave = -1;


    private void Start()
    {
        StartSpawning();
    }

    void Update()
    {
        if (!IsSpawning)
            return;

        WavesTimeProgresss = Time.time - StartedAtTimestamp;

        int? currentWave = GetCurrentWaveIndexByWaveTime();

        if (!currentWave.HasValue)
            return;

        if (LastSpawnedWave >= currentWave.Value)
            return;

        SpawnWaveEnemies(currentWave.Value);
        LastSpawnedWave = currentWave.Value;
    }

    public void StartSpawning()
    {
        LastSpawnedWave = -1;
        StartedAtTimestamp = Time.time;
        IsSpawning = true;
    }

    public void PauseSpawning()
    {
        IsSpawning = false;
    }

    public void ResumeSpawning()
    {
        IsSpawning = true;
    }

    private void SpawnWaveEnemies(int waveIndex)
    {
        WaveConfig config = WaveConfig[waveIndex];

        for (int i = 0; i < config.EnemiesCount; i++)
        {
            GameObject enemy = Instantiate(config.EnemyPrefab, GetRandomPointInASpawnRadius(), Quaternion.identity);

            WaveConfig[waveIndex].SpawnedEnemies.Add(enemy);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, SpawnRadius);
    }

    int? GetCurrentWaveIndexByWaveTime()
    {
        if (WavesTimeProgresss <= 0.0f)
            return null;

        float totalTime = 0.0f;
        for (var i = 0; i < WaveConfig.Length; i++)
        {
            var waveConfig = WaveConfig[i];
            if (WavesTimeProgresss > totalTime && WavesTimeProgresss < totalTime + waveConfig.WaveDuration)
                return i;

            totalTime += waveConfig.WaveDuration;
        }

        return null;
    }

    Vector3 GetRandomPointInASpawnRadius()
    {
        float angle = Random.Range(0.0f, 360.0f);

        return new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad) * SpawnRadius,
            Mathf.Sin(angle * Mathf.Deg2Rad) * SpawnRadius
        );
    }
}