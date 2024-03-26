using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrets : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] GameObject turretHead;

    [Header("----- Turret Stats -----")]
    [Range(0, 50)][SerializeField] int health;
    [SerializeField] Transform[] cannonBarrels;
    [SerializeField] GameObject bullet;
    [Range(0, 5)][SerializeField] float shootRate;
    [Range(1, 10)][SerializeField] int scrapCost;
    [Range(0, 5)][SerializeField] int turretRotateSpeed;

    Color startColor = Color.white;
    bool isShooting;
    int currentCannon;

    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        currentCannon = 0;
        startColor = model.material.color;
        originalMaterial = model.material;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null) {
            Vector3 targetDir = target.position - turretHead.transform.position;
            Quaternion rot = Quaternion.LookRotation(new Vector3(targetDir.x, transform.position.y, targetDir.z));
            turretHead.transform.rotation = Quaternion.Lerp(turretHead.transform.rotation, rot, Time.deltaTime * turretRotateSpeed);

            if (!isShooting) {
                StartCoroutine(FireCannon());
            }
        }
    }

    IEnumerator FireCannon()
    {
        isShooting = true;
        Instantiate(bullet, cannonBarrels[currentCannon].position, turretHead.transform.rotation);

        currentCannon++; // increment to next barrel for next shot,
        if (currentCannon == cannonBarrels.Length) { // If we are at num barrels.length, cycle back to 0
            currentCannon = 0;
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int amount) {
        health -= amount;
        StartCoroutine(flashRed());
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed() {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = startColor;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.isTrigger) {
            return;
        }
        //Debug.Log(other.gameObject.name);

        if (other.CompareTag("Enemy"))
            target = other.GetComponent<Transform>();

        //target = other.GetComponent<Transform>();

    }

    private void OnTriggerExit(Collider other) {
        //if (other.isTrigger) {
        //    return;
        //}
        //target = null;
    }
}
