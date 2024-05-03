using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;
    //[SerializeField] AudioSource gunshots;
    [SerializeField] EffectableObjects Effectable;
    [Header("----- Player Stats -----")]
    [Range(0, 10)][SerializeField] float health;
    [Range(1, 10)][SerializeField] int maxHealth;
    [SerializeField] float speed;
    [Range(1, 4)][SerializeField] float sprintMod;
    [Range(1, 3)][SerializeField] int jumps;
    [Range(5, 25)][SerializeField] int jumpSpeed;
    [Range(-15, -35)][SerializeField] int gravity;
    [SerializeField] int currentAmmo = 0;
    [SerializeField] int maxAmmo = 30;
    bool isImmune = false;
    float immunityDuration = 0.8f;
    public List<GameObject> collectedItems = new List<GameObject>();
    public int healthPickupAmount = 10;
    public int keysCollected = 0;
    public int rocketPiecesCollected = 0;
    public int ammoPickupAmount = 30;
    public int pickupCollected = 0;
    public float jumpForce = 5f;
    public float maxJumpForce = 10f;
    public float minJumpForce = 5f;
    public float jumpTime = 0.5f;
    private bool isJumping = false;
    private float jumpTimeCounter = 0f;


    [Header("----- Gun Stats -----")]
    //[Range(0, 5)][SerializeField] int shootDamage = 0;
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
    [SerializeField] AudioClip deathSound;
    [Range(0f, 1f)][SerializeField] float deathVol;
    [SerializeField] AudioClip shootSound;
    [Range(0f, 1f)][SerializeField] float shootVol;
    [SerializeField] AudioClip rocketSound;
    [Range(0f, 1f)][SerializeField]float rocketVol;
    [SerializeField] AudioClip repairKitSound;
    [Range(0f, 1f)][SerializeField]float repairKitVol;
    [SerializeField] AudioClip reloadSound;



    int jumpCount;
    //Vector3 moveDir;
    Vector3 playerVel;
    bool isShooting;
    bool playingSteps;

    public PlayerInputActions pia; 

    public Vector3 MoveDir { get; set; } = Vector3.zero;

    public int GetNumKeys() { return keysCollected; }
    public int GetCurrentAmmoCount() { return currentAmmo; }
    public int GetMaxAmmoCount() { return maxAmmo; }

    public bool IsJumping
    {
        get; set;
    }

    public bool IsSprinting
    {
        get; set;
    }

    //public bool IsShooting { get; set; }

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
        controller = GetComponent<CharacterController>();
        pia = GetComponent<PlayerInputFunctions>().playerInputActions;
        gameManager.instance.InitializeTurretUI();
        gameManager.instance.SetSelectedTurretUI(selectedTurret.GetComponent<Turrets>(), 0);
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
            //selectTurret();

            if (pia.Player.Shoot.IsPressed() && !isShooting) {
                StartCoroutine(Shoot());
            }
        }
    }

    void Awake()
    {
        Effectable = GetComponent<EffectableObjects>();
    }

    void Movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        Vector3 moveDir;
        moveDir = MoveDir.x * transform.right
                + MoveDir.z * transform.forward;

        if(Effectable != null) {
            controller.Move(moveDir * Effectable.Effect_Speed(speed) * Time.deltaTime);
        }
        else {
            controller.Move(moveDir * speed * Time.deltaTime);
        }

        if (IsJumping && !isJumping && jumpCount < jumps) {
            isJumping = true;
            jumpTimeCounter = 0f;
            jumpCount++;
            //aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
            VolumeControl.Instance.GetSFXAudSrc.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
        }

        if (IsJumping)
        {
            if (jumpTimeCounter < jumpTime) {
                float currentJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, jumpTimeCounter / jumpTime);
                playerVel.y = currentJumpForce;
                jumpTimeCounter += Time.deltaTime;
            }
            else {
                isJumping = false;
            }
        }
        else {
            isJumping = false;
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
        //VolumeControl.Instance.GetSFXAudSrc.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);

        if (IsSprinting)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        playingSteps = false;
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        //IsShooting = false; // Property for InputActions to trigger a shot.
        
        if(Effectable != null) {
            yield return new WaitForSeconds(Effectable.Effect_Blind(shootRate));
        }
        else {
            yield return new WaitForSeconds(shootRate);
        }
        
        isShooting = false;

        if (currentAmmo > 0) 
        {
            isShooting = true;
            GameObject firedBullet = Instantiate(selectedBullet, shootPos.position, Camera.main.transform.rotation);
            firedBullet.GetComponent<Bullet>().ownerTag = Bullet.OwnerTag.Player;
            //Instantiate(selectedBullet, shootPos.position, transform.rotation);
            aud.PlayOneShot(shootSound, shootVol);
            //VolumeControl.Instance.GetSFXAudSrc.PlayOneShot(shootSound, shootVol);
            currentAmmo--;
            gameManager.instance.UpdateAmmoCount();
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
            //VolumeControl.Instance.GetSFXAudSrc.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
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
            //aud.PlayOneShot(reloadSound, aud.volume);
            VolumeControl.Instance.GetSFXAudSrc.PlayOneShot(reloadSound, aud.volume);
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
                //aud.PlayOneShot(repairKitSound, repairKitVol);
                VolumeControl.Instance.GetSFXAudSrc.PlayOneShot(repairKitSound, repairKitVol);

            }
        }

        else if (other.CompareTag("CollectablePickup"))
        {
            PickupCollectable(other.gameObject);
        }


        else if (other.CompareTag("PlayerBase"))
        {
            if (hasRocketPiece)
            {
                RemoveRocketPiece();
                //aud.PlayOneShot(rocketSound, rocketVol);
                VolumeControl.Instance.GetSFXAudSrc.PlayOneShot(rocketSound, rocketVol);
                gameManager.instance.rocketPiecesCollected++;
                gameManager.instance.updateRocketPiecesUI();
                gameManager.instance.UpdateRepairKitsHeld();
            }
        }
    }
    
    void PickUpRocket(GameObject rocket)
    {
        hasRocketPiece = true;
        rocketPiecesCollected++;
        gameManager.instance.updateRocketPiecesUI();
        gameManager.instance.UpdateRepairKitsHeld(true);
        Destroy(rocket);
    }

    void RemoveRocketPiece()
    {
        hasRocketPiece = false;

        rocketPiecesCollected--;
    }

    void PickUpHealth(GameObject healthPickup)
    {
        LerpTowardsPlayer lerpObject = healthPickup.AddComponent<LerpTowardsPlayer>();
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
        LerpTowardsPlayer lerpObject = ammo.AddComponent<LerpTowardsPlayer>();
        currentAmmo += ammoPickupAmount;
        if ( currentAmmo > maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
        gameManager.instance.UpdateAmmoCount();
        Destroy(ammo);
    }

    void PickUpKey(GameObject keyPickup)
    {
        keysCollected++;
        Destroy(keyPickup);
        gameManager.instance.UpdateKeysHeld();
    }

    void PickupCollectable(GameObject collectable)
    {
        LerpTowardsPlayer lerpObject = collectable.AddComponent<LerpTowardsPlayer>();

        pickupCollected++;
        Destroy(collectable);
    }

    public bool HasKey()
    { return keysCollected > 0; }



    public bool HasRocketPiece()
    {
        return rocketPiecesCollected > 0;
    }



    public void selectTurret(int direction)
    {
        if(direction != 0) {
            int currentIndex = turrets.IndexOf(selectedTurret);
            currentIndex += (int)Mathf.Sign(direction);
            if (currentIndex < 0) {
                currentIndex = turrets.Count - 1;
            }
            else if (currentIndex >= turrets.Count) {
                currentIndex = 0;
            }
            SetSelectedTurret(currentIndex);
        }
    }

    public void SetSelectedTurret(int index) {
        selectedTurret = turrets[index];
        gameManager.instance.SetSelectedTurretUI(selectedTurret.GetComponent<Turrets>(), index);
    }

    public void PlaceTurret() {
        int turretCost = selectedTurret.GetComponent<Turrets>().GetTurretCost();
        if (selectedTurret.GetComponent<Turrets>().GetTurretCost() <= gameManager.instance.scrapWallet) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, turretPlacementDist)) {
                Vector3 placeOnGround = new Vector3(hit.point.x, 0, hit.point.z);
                Instantiate(turretBuilder, placeOnGround, transform.rotation);
                gameManager.instance.RemoveScrap(selectedTurret.GetComponent<Turrets>().GetTurretCost());
            }
        }
        else {
            // gamemanager.instance.insufficentfunds call
        }
    }
}


