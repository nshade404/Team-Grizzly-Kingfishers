using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Wavespawner : MonoBehaviour
//{
//    public List<Enemy> enemies = new List<Enemy>();
//    public int currWave;
//    public int waveValue;
//    public List<GameObject> enemiesToSpawn = new List<GameObject>();

//    public Transform spawnLocation;
//    public int waveDuration;
//    private float waveTimer;
//    private float spawnInterval;
//    private float spawnTimer;

//    // Start is called before the first frame update
//    void Start()
//    {
//        GenerateWaves();
        
//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {
//        if(spawnTimer <= 0)
//        {
//            if(enemiesToSpawn.Count > 0)
//            {
//                Instantiate(enemiesToSpawn[0], spawnLocation.position, Quaternion.identity);
//                enemiesToSpawn.RemoveAt(0);
//                spawnTimer = spawnInterval;
//            }
//            else { waveTimer = 0; }


//        }
//        else
//        {
//            spawnTimer -= Time.deltaTime;
//            waveTimer -= Time.deltaTime;
//        }



        
//    }

//    public void GenerateWaves()
//    {
//        waveValue = currWave * 10;
//        GenerateEnemies();

//        spawnInterval = waveDuration / enemiesToSpawn.Count;
//        waveTimer = waveDuration;
//    }

    
//   public void GenerateEnemies()
//    {
//        if (enemies.Count == 0)
//        {
//            Debug.LogError("No enemies available for spawning.");
//            return;
//        }

//        List<GameObject> generatedEnemies = new List<GameObject>();
//        int totalCost = waveValue;

//        while (totalCost > 0)
//        {
//            int randEnemyId = Random.Range(0, enemies.Count);
//            int randEnemyCost = enemies[randEnemyId].cost;

//            if (totalCost - randEnemyCost >= 0)
//            {
//                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
//                totalCost -= randEnemyCost;
//            }
//            else if (totalCost <= 0)
//            {
//                break;
//            }
//        }

//        enemiesToSpawn.Clear();
//        enemiesToSpawn.AddRange(generatedEnemies);
//    }


//}



//[System.Serializable]
//public class Enemy
//{
//    public GameObject enemyPrefab;
//    public int cost;

//}
