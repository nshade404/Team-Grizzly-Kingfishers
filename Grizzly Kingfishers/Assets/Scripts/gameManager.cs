using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class gameManager : MonoBehaviour {
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject objectivePopup;
    [SerializeField] TMP_Text rocketPiecesCollectedText;
    [SerializeField] TMP_Text heldRocketPiecesText;
    [SerializeField] TMP_Text scrapText;
    [SerializeField] GameObject enemySpawn;
    [SerializeField] TurretDetailsUI selectedTurretDetailsUI;
    public float spawnTime;
    public float spawnDelay;
    public int enemiesPerWave;


    [Header("----- Healthbar UI Items -----")]
    [SerializeField] Image playerHPBar;
    [SerializeField] Image rocketHPBar;
    [SerializeField] GameObject playerDamageFlash;

    public GameObject player;
    public playerController playerScript;
    public GameObject playerBase;

    public bool isStartOfGame;
    public bool isPaused;
    public bool isSpawning;
    float timeScaleOrig;
    int enemyCount;
    public int enemyWaveCount;
    public int maxEnemies = 50;

    public GameObject optionScreen;
    public GameObject loadingScreen;

    [SerializeField] Transform enemySpawnPoint;

    public int rocketPiecesCollected = 0;
    public int rocketPiecesRequired = 3;

    public int scrapWallet = 0;
    public int turretCostAmount;

    [Header("----- Turret UI Items -----")]
    [SerializeField] List<TurretButton> turretButtons;
    [SerializeField] Sprite turretBtnIconIdle;
    [SerializeField] Sprite turretBtnIconSelected;

    // Start is called before the first frame update
    void Awake() {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        timeScaleOrig = Time.timeScale;
        StartCoroutine(startingPopup());
        playerBase = GameObject.FindWithTag("PlayerBase");

        updateRocketPiecesUI();
        UpdateRepairKitsHeld();
        UpdateScrapUI();
    }

    private void Start() {
        InitializeTurretUI();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel") && menuActive == null) {
            statePaused();
            menuActive = menuPause;
            menuActive.SetActive(isPaused);
        }
    }

    public void statePaused() {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpaused() {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    IEnumerator startingPopup() {
        menuActive = objectivePopup;
        menuActive.SetActive(true);
        yield return new WaitForSeconds(7.5f);
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal(int amount) {
        enemyCount += amount;
    }
    public void youHaveLost() {
        statePaused();
        if (menuLose != null) {
            menuActive = menuLose;
            menuActive.SetActive(true);
        }
        else {
            Debug.Log("gameManager.menuLose not set!");
        }
    }

    public void youHaveWon() {
        statePaused();
        if (menuWin != null) {
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }

    #region Health Bar and Damage Updates
    public void flashPlayerDamage(bool isWidgetActive) {
        if (playerDamageFlash != null) {
            playerDamageFlash.SetActive(isWidgetActive);
        }
        else {
            Debug.Log("gameManager.playerDamageFlash is null!");
        }
    }

    public void updatePlayerHealthBar(float amount) {
        if (playerHPBar != null) {
            playerHPBar.fillAmount = amount;
        }
        else {
            Debug.Log("gameManager.playerHPBar not set!");
        }
    }

    public void updateRocketHealthBar(float amount) {
        if (rocketHPBar != null) {
            rocketHPBar.fillAmount = amount;
        }
        else {
            Debug.Log("gameManager.rocketHPBar not set!");
        }
    }
    #endregion

    public void updateRocketPiecesUI() {
        if (rocketPiecesCollectedText != null) {
            rocketPiecesCollectedText.text = string.Format("[{0}/{1}]", rocketPiecesCollected, rocketPiecesRequired);
        }
        else {
            Debug.Log("gameManager.rocketPiecesCollectedText not set!");
        }

        if (rocketPiecesCollected == rocketPiecesRequired) {
            // Update this to trigger the 'final' wave when we get to setting that up...
            youHaveWon();
            //updateGameGoal(-1);
        }
    }

    public void UpdateRepairKitsHeld(bool hasPiece = false) {
        if (hasPiece) {
            heldRocketPiecesText.text = "1/1";
        }
        else {
            heldRocketPiecesText.text = "0/1";
        }
    }

    public void AddScrap(int amount) {
        scrapWallet += amount;
        UpdateScrapUI();
    }

    public void RemoveScrap(int amount) {
        scrapWallet -= amount;
        UpdateScrapUI();
    }

    void UpdateScrapUI() {
        if (scrapText != null) {
            scrapText.text = scrapWallet.ToString();
        }
    }

    #region Turret Selection and Display

    public void SetSelectedTurretUI(Turrets turret, int index) {
        DeselectAllTurrets();
        turretButtons[index].btnBackground.sprite = turretBtnIconSelected;
        selectedTurretDetailsUI.displayName.text = turret.GetDisplayName();
        selectedTurretDetailsUI.ammoType.text = turret.GetBulletType()?.GetDamageType().ToString();
        selectedTurretDetailsUI.shootRate.text = turret.GetShootRate().ToString("F2");
        selectedTurretDetailsUI.rotSpeed.text = turret.GetRotationSpeed().ToString();
        selectedTurretDetailsUI.cost.text = turret.GetTurretCost().ToString();
    }

    private void InitializeTurretUI() {
        for(int i = 0; i < playerScript.turrets.Count; i++) {
            // Safety check in case some reason player has more turrets in list
            if(i >= turretButtons.Count) { break; }

            turretButtons[i].btnBackground.sprite = turretBtnIconIdle;
            turretButtons[i].icon.sprite = playerScript.turrets[i].GetComponent<Turrets>().turretIcon;
            turretButtons[i].cost.text = playerScript.turrets[i].GetComponent<Turrets>().GetTurretCost().ToString();
        }
    }

    private void DeselectAllTurrets() {
        foreach(TurretButton btn in turretButtons) {
            btn.btnBackground.sprite = turretBtnIconIdle;
        }
    }

    #endregion
}
