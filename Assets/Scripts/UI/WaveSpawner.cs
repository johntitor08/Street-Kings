using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    private readonly float timeBetweenWaves = 10f;
    private int waveIndex = 0;

    private void Start()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("WaveSpawner: Enemy prefabs atanmamış!", this);
            return;
        }

        InvokeRepeating(nameof(SpawnWave), 5f, timeBetweenWaves);
    }

    private void SpawnWave()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("WaveSpawner: Spawn point atanmamış!", this);
            return;
        }

        waveIndex++;

        for (int i = 0; i < 3; i++)
        {
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

            if (sp == null)
                continue;

            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            if (prefab == null)
                continue;

            Instantiate(prefab, sp.position, Quaternion.identity);
        }
    }
}
