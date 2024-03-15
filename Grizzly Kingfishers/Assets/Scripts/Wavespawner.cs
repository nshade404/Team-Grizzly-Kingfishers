using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Wavespawner : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] private GameObject spawnLocation;

    public Wave[] waves;

    public int waveCount;

    private void Start()
    {
      for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemies.Length;
        }
        
    }

    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            countdown = waves[waveCount].timeToNextWave;
            StartCoroutine(SpawnWave());

        }
        if (waves[waveCount].enemiesLeft ==0)
        {
            waveCount++;
        }
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < waves[waveCount].enemies.Length; i++) 
        {

            EnemyAI enemy = Instantiate(waves[waveCount].enemies[i], spawnLocation.transform);

            enemy.transform.SetParent(spawnLocation.transform);
        }

        yield return new WaitForSeconds(waves[waveCount].timeToNextEnemy);
    }
}

[System.Serializable]

public class Wave
{
    public EnemyAI[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemiesLeft;
}