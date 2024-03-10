using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage {
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject player; // temporary reference to the player until gameManager is up and running.

    [Header("----- Enemy Stats -----")]
    [Range(0, 10)][SerializeField] int health;
    [Range(0, 5)][SerializeField] int speed;
    [Range(0, 5)][SerializeField] int wanderWaitTime;
    [Range(0, 5)][SerializeField] float wanderDist = 5.0f;
    [Range(0, 50)][SerializeField] float aggroDist;

    [Header("----- Weapon Stats -----")]
    [Range(0, 1)][SerializeField] float shootRate;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;

    [Header("----- Debug Testing -----")]
    [SerializeField] bool disableEnemy;

    Color startColor = Color.white;
    bool isShooting;
    Vector3 wanderPos;
    bool isWandering;

    // Start is called before the first frame update
    void Start() {
        if (model != null) // capture initial material color.
            startColor = model.material.color;

        // Fire off a timer that waits for 1 second to make sure player is loaded.
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        // Debug key (default F5) to disable enemies on the fly for testing purposes,
        // turns off their shooting and movement.
        if (Input.GetButtonDown("DisableEnemy")) {
            disableEnemy = !disableEnemy;
        }

        if(player != null && !disableEnemy) {
            // Aware this likely isn't the most effective way to perform this and probably should put on a timer
            // But doing for now to get the logic working.
            float distanceToPlayer = Mathf.Abs(Vector3.Distance(transform.position, player.transform.position));
            if (distanceToPlayer <= aggroDist) { // we see the player, move towards and attack...
                agent.SetDestination(player.transform.position);
                if (!isShooting) {
                    StartCoroutine(Shoot());
                }
            }
            else { // wander around a little bit.

                if (!isWandering) {
                    StartCoroutine(Wander());
                }
                else {
                    float distance = Mathf.Abs(Vector3.Distance(transform.position, wanderPos));
                    if(distance >= 0.1f) {
                        agent.SetDestination(wanderPos);
                    }
                }
            }
        }
    }

    IEnumerator Wander() {
        isWandering = true;
        float newX = Random.Range(-wanderDist, wanderDist) + transform.position.x;
        float newZ = Random.Range(-wanderDist, wanderDist) + transform.position.z;
        wanderPos = new Vector3(newX, 0, newZ);

        yield return new WaitForSeconds(wanderWaitTime);
        isWandering = false;
    }

    IEnumerator Shoot() {
        isShooting = true;
        if(bullet != null) {
            Instantiate(bullet, shootPos.position, transform.rotation);
        } else {
            Debug.Log(gameObject + " is firing!");
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        health -= amount;
        StartCoroutine(flashRed());

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = startColor;
    }
}
