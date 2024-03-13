using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] TMP_Text enemyCountText;
    public Image playerHPBar;
    public GameObject playerDamageFlash;

    public GameObject player;
    public playerController playerScript;

    public bool isPaused;
    float timeScaleOrig;
    int enemyCount;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        timeScaleOrig = Time.timeScale;
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

        if(enemyCountText != null) {
            enemyCountText.text = enemyCount.ToString("F0");
        }

        if (enemyCount <= 0)
        {
            statePaused();
            if(menuWin != null) {
                menuActive = menuWin;
                menuActive.SetActive(true);
            } else {
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
}
