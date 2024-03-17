using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] int spawnDist;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawnEnemies(int numToSpawn) {
        StartCoroutine(SpawnEnemies(numToSpawn));
    }

    IEnumerator SpawnEnemies(int numToSpawn) {

        for(int i = 0; i < numToSpawn; i++) {
            Vector3 spawnPos = Random.insideUnitSphere * spawnDist;
            spawnPos += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(spawnPos, out hit, spawnDist, 1);

            Instantiate(GetRandomEnemyType(), hit.position, Quaternion.identity);   
        }

        yield return null;
    }

    GameObject GetRandomEnemyType() {
        GameObject retObj = null;

        int selection = Random.Range(0, enemyPrefabs.Length); // Return us a number between 0 and 100
        retObj = enemyPrefabs[selection];

        return retObj;
    }
}
