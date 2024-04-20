using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;

    [Header("----- Player Stats -----")]
    [Range(0, 10)][SerializeField] float health;
    [Range(1, 10)][SerializeField] int maxHealth;
    [Range(1, 5)][SerializeField] float speed;
    [Range(1, 4)][SerializeField] float sprintMod;
    [Range(1, 3)][SerializeField] int jumps;
    [Range(5, 25)][SerializeField] int jumpSpeed;
    [Range(-15, -35)][SerializeField] int gravity;
    [SerializeField] int currentAmmo = 0;
    [SerializeField] int maxAmmo = 30;
    bool isImmune = false;
    float immunityDuration = 2.0f;
    public List<GameObject> collectedItems = new List<GameObject>();
    public int healthPickupAmount = 10;
    public int keysCollected = 0;
    public int rocketPiecesCollected = 0;
    public int ammoPickupAmount = 30;
    public float jumpForce = 5f;
    public float maxJumpForce = 10f;
    public float minJumpForce = 5f;
    public float jumpTime = 0.5f;
    private bool isJumping = false;
    private float jumpTimeCounter = 0f;


    [Header("----- Gun Stats -----")]
    [Range(0, 5)][SerializeField] int shootDamage;
    [Range(0, 100)][SerializeField] int shootDist;
    [Range(0, 1)][SerializeField] float shootRate;
    [SerializeField] GameObject selectedBullet;
    [SerializeField] Transform shootPos;


    [Header("----- Turret Stats -----")]
    public List<GameObject> turrets;
    public GameObject selectedTurret;
    [SerializeField] GameObject turretBuilder;
    [SerializeField] int turretPlacementDist;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;

    int jumpCount;
    //Vector3 moveDir;
    Vector3 playerVel;
    bool isShooting;
    bool playingSteps;

    public Vector3 MoveDir { get; set; } = Vector3.zero;

    public bool IsJumping
    {
        get; set;
    }

    public bool IsSprinting
    {
        get; set;
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public float GetSprintMod { get { return sprintMod; } }



    // Start is called before the first frame update
    void Start()
    {
        updatePlayerUI();
        selectedTurret = turrets.First();
        gameManager.instance.SetSelectedTurretUI(selectedTurret.GetComponent<Turrets>(), 0);
        //gameManager.instance.costOfTurret(selectedTurret.name, selectedTurret.GetComponent<Turrets>().GetTurretCost()); // Update selected turret on startup.

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
#if UNITY_EDITOR 
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.blue);
#endif

            Movement();
            selectTurret();

            if (Input.GetButton("Shoot") && !isShooting)
            {
                StartCoroutine(Shoot());
            }
            if (Input.GetButtonDown("PlaceTurret"))
            {
                int turretCost = selectedTurret.GetComponent<Turrets>().GetTurretCost();
                if (selectedTurret.GetComponent<Turrets>().GetTurretCost() <= gameManager.instance.scrapWallet)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, turretPlacementDist))
                    {
                        Vector3 placeOnGround = new Vector3(hit.point.x, 0, hit.point.z);
                        Instantiate(turretBuilder, placeOnGround, transform.rotation);
                        gameManager.instance.RemoveScrap(selectedTurret.GetComponent<Turrets>().GetTurretCost());
                    }
                }
                else
                {
                    // gamemanager.instance.insufficentfunds call
                }
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
        Vector3 moveDir;
        //moveDir = new Vector3(MoveDir.x * transform.right, 0, MoveDir.z * transform.forward);
        //moveDir = Input.GetAxis("Horizontal") * transform.right
        //        + Input.GetAxis("Vertical") * transform.forward;
        moveDir = MoveDir.x * transform.right
                + MoveDir.z * transform.forward;

        controller.Move(moveDir * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < jumps)
        {
            isJumping = true;
            jumpTimeCounter = 0f;
            jumpCount++;
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
        }

        if (isJumping)
        {
            if (Input.GetKey(KeyCode.Space) && jumpTimeCounter < jumpTime)
            {
                float currentJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, jumpTimeCounter / jumpTime);
                playerVel.y = currentJumpForce;
                jumpTimeCounter += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        
        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);

        if (controller.isGrounded && moveDir.normalized.magnitude > 0.3f && !playingSteps)
        {
            StartCoroutine(playSteps());
        }
    }



    IEnumerator playSteps()
    {
        playingSteps = true;
        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);

        if (IsSprinting)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }
        playingSteps = false;
    }

    IEnumerator Shoot()
    {
        if (currentAmmo > 0) 
        {
            isShooting = true;
            Instantiate(selectedBullet, shootPos.position, transform.rotation);
            currentAmmo--; 
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
        else
        {
            
            Debug.Log("Out of ammo!");
        }
    }

    public void takeDamage(float amount)
    {
        if (!isImmune)
        {
            health -= amount;
            aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
            StartCoroutine(flashDamageScreen());
            updatePlayerUI();

            if (health <= 0)
            {
                gameManager.instance.youHaveLost();
            }

            StartCoroutine(ApplyImmunity());
        }
    }
    IEnumerator ApplyImmunity()
    {
        isImmune = true;
        yield return new WaitForSeconds(immunityDuration);
        isImmune = false;
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
        
        else if (other.CompareTag("AmmoPickup"))
        {
            PickupAmmo(other.gameObject);
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
        else if (other.CompareTag("PlayerBase"))
        {

            if (hasRocketPiece)
            {
                RemoveRocketPiece();
                gameManager.instance.rocketPiecesCollected++;
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

    void PickupAmmo(GameObject ammo)
    {
        currentAmmo += ammoPickupAmount;
        if ( currentAmmo > maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
        Destroy(ammo);
    }

    void PickUpKey(GameObject keyPickup)
    {
        keysCollected++;
        Destroy(keyPickup);

    }

    public bool HasKey()
    { return keysCollected > 0; }



    public bool HasRocketPiece()
    {
        return rocketPiecesCollected > 0;
    }



    public void selectTurret()
    {
        float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheelInput != 0)
        {
            int currentIndex = turrets.IndexOf(selectedTurret);
            currentIndex += (int)Mathf.Sign(mouseWheelInput);
            if (currentIndex < 0)
            {
                currentIndex = turrets.Count - 1;
            }
            else if (currentIndex >= turrets.Count)
            {
                currentIndex = 0;
            }
            selectedTurret = turrets[currentIndex];

            gameManager.instance.SetSelectedTurretUI(selectedTurret.GetComponent<Turrets>(), currentIndex);
            
            //int turretCost = selectedTurret.GetComponent<Turrets>().GetTurretCost();
            //gameManager.instance.costOfTurret(selectedTurret.name, turretCost);
        }
    }

}


