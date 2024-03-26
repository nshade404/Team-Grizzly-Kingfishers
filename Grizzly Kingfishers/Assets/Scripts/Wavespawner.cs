using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;



public class Wavespawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    public SpawnState state = SpawnState.COUNTING;

    public GameObject[] enemyPrefabs;
    public float timeBetweenWaves = 5f;
    public float countDown = 2f;
    public int maxEnemyCount = 5;
    public int enemiesPerWave = 3; // Number of enemies to spawn per wave
    public Transform[] spawnPoints;

    private List<GameObject> enemies = new List<GameObject>();
    private int waveIndex = 0;

    private void Start()
    {
        StartCoroutine(RunSpawner());
    }

    private IEnumerator RunSpawner()
    {
        yield return new WaitForSeconds(countDown);

        while (true)
        {
            state = SpawnState.SPAWNING;

            yield return SpawnWave();

            state = SpawnState.WAITING;

            yield return new WaitWhile(EnemyIsAlive);

            state = SpawnState.COUNTING;

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private bool EnemyIsAlive()
    {
        enemies = enemies.Where(e => e != null).ToList();
        return enemies.Count > 0;
    }

    private IEnumerator SpawnWave()
    {
        int numEnemiesToSpawn = enemiesPerWave + (Random.Range(1, enemiesPerWave + 1) * waveIndex);

        for (int i = 0; i < numEnemiesToSpawn; i++)
        {
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            SpawnEnemy(randomSpawnPoint);
            yield return new WaitForSeconds(0.5f);

            if (enemies.Count < maxEnemyCount)
            {
                
            }
            else
            {
                yield return null;
            }
        }
        waveIndex++;
    }

    private void SpawnEnemy(Transform spawnPoint)
    {
        enemies.Add(Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoint.position, spawnPoint.rotation));
    }
}
