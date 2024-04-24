using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage {
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] EffectableObjects Effectable;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource aud;

    [Header("----- Enemy Stats -----")]
    [Range(0, 10)][SerializeField] float health;
    [Range(0, 5)][SerializeField] int speed;

    [Header("----- Weapon Stats -----")]
    [Range(0, 5)][SerializeField] float shootRate;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;

    [Header("----- Detection Settings -----")]
    [SerializeField] Transform headPos;
    [SerializeField] float aggroDist; // the size of our aggro collider
    [SerializeField] int viewCone; // our field of view
    [SerializeField] int faceTargetSpeed; // Spped in which we face the player when not moving.

    [Header("----- Drops -----")]
    [SerializeField] int minScrapDrop;
    [SerializeField] int maxScrapDrop;
    [SerializeField] GameObject healthDrop;
    [Range(0,100)][SerializeField] int healthDropChance;

    [Header("----- Drops -----")]
    [SerializeField] AudioClip[] audInsult;
    [Range(0, 1)][SerializeField] float audInsultVol;

    [Header("----- Debug Testing -----")]
    [SerializeField] bool disableEnemy;

    Color startColor = Color.white;
    bool isShooting;
    bool isAlive;
    bool isMoving;
    bool destinationChosen; // Tracks if we are in wander mode or not.
    float originalStoppingDistance; // used to capture our defined stopping distance on nav mesh agent.
    Vector3 startingPos; // captures our starting position for our wandering functionality.

    Vector3 playerDir; // Tracks the direction to the player.
    float angleToPlayer; // current angle player is to relative to us
    public bool playerInRange; // Tracks whether or not we are actively tracking the player for aggro.

    public Transform target;
    public List<Transform> targets = new List<Transform>(); // Holds our list of targets that came into our attack range.
    [SerializeField] AudioClip audShoot;
    [Range(0, 1)][SerializeField] float audShootVol;

    public bool IsAlive { get { return isAlive; } }

    // Start is called before the first frame update
    void Start() {
        gameManager.instance.updateGameGoal(1);
        target = null;

        if (model != null) { // capture initial material color.
            startColor = model.material.color;
        } else {
            Debug.Log("Forgot to set reference for 'model' in EnemyAI!!");
        }

        // Set our aggro collider to the radius defined.
        GetComponentInChildren<SphereCollider>().radius = aggroDist;
        originalStoppingDistance = agent.stoppingDistance;
        startingPos = transform.position;

        agent.SetDestination(gameManager.instance.playerBase.transform.position);
        isAlive = true;
    }

    // Update is called once per frame
    void Update() {
        // Debug key (default F5) to disable enemies on the fly for testing purposes,
        // turns off their shooting and movement.
        if (Input.GetButtonDown("DisableEnemy")) {
            disableEnemy = !disableEnemy;
        }

#if UNITY_EDITOR // Allow us to adjust the size of the collider in editor if we are trying out differen sizes.
        if(aggroDist != GetComponentInChildren<SphereCollider>().radius) {
            // If we changed in editor for testing, go ahead and update to the new value.
            GetComponentInChildren<SphereCollider>().radius = aggroDist;
        }
#endif
        // Debugging functionality to prevent enemies from doing actions while we are testing.
        if (disableEnemy) {
            return;
        }

        if (isAlive) {
            CanSeePlayer();
        }
        //if (playerInRange && !CanSeePlayer()) {
        //}
        //else if (!playerInRange) {
        //}
        
        // section to add idle animation to the enemy model
        if (agent.velocity.magnitude <= 0.01f && isAlive)
        {
            anim.SetBool("isMoving", false);
        }
        else if (agent.velocity.magnitude > 0.01f && isAlive)
        {
            anim.SetBool("isMoving", true);
        }
    }

    bool CanSeePlayer() {
        if(target == null) {
            if (targets.Count > 0) {
                if (targets.First() == null) {
                    targets.RemoveAt(0);
                }
                if (targets.Count > 0) { // We still have targets to select from
                    target = targets.First();
                }
                else { // Otherwise just empty target for now. and mark as can't see.
                    target = null;
                }
            } 
        }
        else {
            if(isAlive == false) { return false; }

            playerDir = target.position - headPos.position;
            angleToPlayer = Vector3.Angle(playerDir, transform.forward);
            //Debug.Log(angleToPlayer);
            Debug.DrawRay(shootPos.position, playerDir);
            RaycastHit hit;
            if (Physics.Raycast(headPos.position, playerDir, out hit)) {
                //Debug.Log(hit.collider.name);

                bool hitIsTargetable = hit.collider.CompareTag("Player") || hit.collider.CompareTag("Turret") || hit.collider.CompareTag("PlayerBase");

                if (hitIsTargetable && angleToPlayer <= viewCone) {
                    //agent.stoppingDistance = originalStoppingDistance;
                    //agent.SetDestination(target.position);

                    if (!isShooting) {
                        StartCoroutine(Shoot());
                    }

                    if (agent.remainingDistance <= agent.stoppingDistance) {
                        FaceTarget();
                    }

                    return true;
                }
            }
        }
        //agent.stoppingDistance = 0;
        return false;
    }

    void FaceTarget() {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    IEnumerator Shoot() {
        isShooting = true;
       if(bullet != null) {
            Bullet.DamageType type = bullet.GetComponent<Bullet>().GetDamageType();
            if (bullet.GetComponent<Bullet>().GetDamageType() == Bullet.DamageType.visciousMockery) {
                FireInsult();
            } else { 
                Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
                Instantiate(bullet, shootPos.position, rot);
                aud.PlayOneShot(audShoot, audShootVol);
            }
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(float amount)
    {
        health -= amount;
        StartCoroutine(flashRed());
        //agent.SetDestination(gameManager.instance.player.transform.position);
        if (health > 0 && isAlive)
        {
            StartCoroutine(damageAnimation());
        }
        else 
        {
            isAlive = false; // set to false so we no longer run this if gets hit again

            if(isAlive == false) {
                agent.speed = 0;
                isShooting = false;
                StartCoroutine(deathAnimation());
                //gameManager.instance.updateGameGoal(-1);
                gameManager.instance.AddScrap(Random.Range(minScrapDrop, maxScrapDrop));
                int chance = Random.Range(0, 101);
                if (chance <= healthDropChance) {
                    Instantiate(healthDrop, transform.position, transform.rotation);

                    HealthDropFloatingMotion floatingMotion = healthDrop.AddComponent<HealthDropFloatingMotion>();
                }
            }
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = startColor;
    }

    IEnumerator damageAnimation()
    {

        agent.speed = speed;
        int speedSave = speed;
        agent.speed = 0;
        anim.SetTrigger("Damage");
        yield return new WaitForSeconds(1f);
        agent.speed = speedSave;
    }

    IEnumerator deathAnimation()
    {
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    /// <summary>
    /// I realize this is not the most efficient way, but we are having fun with this....
    /// please dont judge.... -Josh
    /// </summary>
    public void FireInsult() {
        int choice = Random.Range(0, 5);
        string saying = "";
        switch (choice) {
            case 0:
                saying = "You sure you don't need glasses?!";
                break;
            case 1:
                saying = "Grandma has more gun stability than you!";
                break;
            case 2:
                saying = "What're you? A Stormtrooper??";
                break;
            case 3:
                saying = "I think your brain is AFK.";
                break;
            case 4:
                saying = "Your father smelled of elderberries!";
                break;
        }

        Debug.Log(saying);
    }
}
