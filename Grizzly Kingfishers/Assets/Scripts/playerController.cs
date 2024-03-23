using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(0, 10)][SerializeField] int health;
    [Range(1, 10)][SerializeField] int maxHealth;
    [Range(1, 5)][SerializeField] float speed;
    [Range(1, 4)][SerializeField] float sprintMod;
    [Range(1, 3)][SerializeField] int jumps;
    [Range(5, 25)][SerializeField] int jumpSpeed;
    [Range(-15, -35)][SerializeField] int gravity;
    public List<GameObject> collectedItems = new List<GameObject>();
    public int healthPickupAmount = 10;
    public int keysCollected = 0;
    public int rocketPiecesCollected = 0;
    

    [Header("----- Gun Stats -----")]
    [Range(0, 5)][SerializeField] int shootDamage;
    [Range(0, 100)][SerializeField] int shootDist;
    [Range(0, 1)][SerializeField] float shootRate;
    [SerializeField] GameObject selectedBullet;
    [SerializeField] Transform shootPos;

    [Header("----- Turret Stats -----")]
    [SerializeField] GameObject selectedTurret;
    [SerializeField] int turretPlacementDist;

    int jumpCount;
    Vector3 moveDir;
    Vector3 playerVel;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
#if UNITY_EDITOR 
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.blue);
#endif

            Sprint();
            Movement();

            if (Input.GetButton("Shoot") && !isShooting)
            {
                StartCoroutine(Shoot());
            }

            if (Input.GetButtonDown("PlaceTurret"))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, turretPlacementDist))
                {
                    Instantiate(selectedTurret, hit.point, transform.rotation);
                }
            }

            if (Input.GetButtonDown("SpawnTest"))
            {
                EnemySpawner spawner = GameObject.FindWithTag("Spawner").GetComponent<EnemySpawner>();
                spawner.StartSpawnEnemies(10);
            }

        }
    }

    void Movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        // 1st person camera controls
        moveDir = Input.GetAxis("Horizontal") * transform.right
                + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(moveDir * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumps)
        {
            playerVel.y = jumpSpeed;
            jumpCount++;
        }

        // Gravity
        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        Instantiate(selectedBullet, shootPos.position, transform.rotation);

        //RaycastHit hit;
        //if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist)) {
        //    Debug.Log(hit.collider.name);

        //    IDamage dmg = hit.collider.GetComponent<IDamage>();
        //    if (hit.transform != transform && dmg != null) {
        //        Debug.Log("Are we getting into the hit.takedamage");
        //        dmg.takeDamage(shootDamage);
        //    }
        //}
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        health -= amount;
        StartCoroutine(flashDamageScreen());
        updatePlayerUI();

        if (health <= 0)
        {
            gameManager.instance.youHaveLost();
        }
    }

    IEnumerator flashDamageScreen()
    {
        gameManager.instance.flashPlayerDamage(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.flashPlayerDamage(false);
    }

    void updatePlayerUI()
    {
        gameManager.instance.updatePlayerHealthBar((float)health / maxHealth);
    }

    bool hasRocketPiece = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthPickup"))
        {
            PickUpHealth(other.gameObject);
        }
        else if (other.CompareTag("Key"))
        {
            PickUpKey(other.gameObject);
        }
        else if (other.CompareTag("RocketPiece"))
        {
            
            if (!hasRocketPiece)
            {
                PickUpRocket(other.gameObject);
            }
        }
        else if (other.CompareTag("StartingRoom")) 
        {
            
            if (hasRocketPiece)
            {
                RemoveRocketPiece();
                gameManager.instance.updateRocketPiecesUI();
            }
        }
    }

    void PickUpRocket(GameObject rocket)
    {
        hasRocketPiece = true; 
        rocketPiecesCollected++;
        gameManager.instance.updateRocketPiecesUI();
        Destroy(rocket);
    }

    void RemoveRocketPiece()
    {
        hasRocketPiece = false; 
       
        rocketPiecesCollected--;
       
        gameManager.instance.updateGameGoal(-1);
    }

    void PickUpHealth(GameObject healthPickup)
    {

        health += healthPickupAmount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }


        Destroy(healthPickup);


        updatePlayerUI();
    }

    void PickUpKey(GameObject keyPickup)
    {
        keysCollected++;
        Destroy(keyPickup);

    }

    public bool HasKey()
    {  return keysCollected > 0; }

    

    public bool HasRocketPiece()
    { 
        return rocketPiecesCollected > 0;
    }

    
}


