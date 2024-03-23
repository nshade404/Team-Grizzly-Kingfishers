using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text rocketPiecesCollectedText;

    [SerializeField] GameObject enemySpawn;
    public float spawnTime;
    public float spawnDelay;
    public int enemiesPerWave;
    
    public Image playerHPBar;
    public GameObject playerDamageFlash;

    public GameObject player;
    public playerController playerScript;

    public bool isPaused;
    public bool isSpawning;
    float timeScaleOrig;
    int enemyCount;
    public int enemyWaveCount;
    public int maxEnemies = 50;
    
    [SerializeField] Transform enemySpawnPoint;

    public int rocketPiecesCollected = 0;
    public int rocketPiecesRequired = 3;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        timeScaleOrig = Time.timeScale;
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && menuActive == null)
        {
            statePaused();
            menuActive = menuPause;
            menuActive.SetActive(isPaused);
        }
    }

    public void statePaused()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpaused()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        enemyCount += amount;

        if (enemyCountText != null)
        {
            enemyCountText.text = enemyCount.ToString("F0");
        }

        if (enemyCount <= 0 && rocketPiecesCollected >= rocketPiecesRequired)
        {
            statePaused();
            if (menuWin != null)
            {
                menuActive = menuWin;
                menuActive.SetActive(true);
            }
            else
            {
                Debug.Log("gameManager.menuWin not set!");
            }
        }
    }
    public void youHaveLost()
    {
        statePaused();
        if(menuLose != null) {
            menuActive = menuLose;
            menuActive.SetActive(true);
        }
        else {
            Debug.Log("gameManager.menuLose not set!");
        }
    }

    public void flashPlayerDamage(bool isWidgetActive) {
        if(playerDamageFlash != null) {
            playerDamageFlash.SetActive(isWidgetActive);
        } else {
            Debug.Log("gameManager.playerDamageFlash is null!");
        }
    }

    public void updatePlayerHealthBar(float amount) {
        if(playerHPBar != null) {
            playerHPBar.fillAmount = amount;
        }
        else {
            Debug.Log("gameManager.playerHPBar not set!");
        }
    }

    public void updateRocketPiecesUI() 
    {
        if (rocketPiecesCollectedText != null)
        {
            rocketPiecesCollectedText.text = rocketPiecesCollected + " / " + rocketPiecesRequired;
        }
        else
        {
            Debug.Log("gameManager.rocketPiecesCollectedText not set!");
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(spawnDelay);

        if (enemyCount < maxEnemies)
        {
            enemiesPerWave = enemyWaveCount * 3;
            for (int i = 0; i < enemiesPerWave; i++)
            {
                Instantiate(enemySpawn, enemySpawnPoint.position, enemySpawnPoint.rotation);
                yield return new WaitForSeconds(spawnTime);
            }
            enemyWaveCount++;
            yield return new WaitForSeconds(spawnDelay);
        }
        else
        {
            yield return null;
        }
    }
}
