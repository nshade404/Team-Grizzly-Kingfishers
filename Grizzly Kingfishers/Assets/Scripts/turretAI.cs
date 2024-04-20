using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Turrets : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] GameObject turretHead;

    [Header("----- Turret Stats -----")]
    [Range(0, 50)][SerializeField] float health;
    [SerializeField] Transform[] cannonBarrels;
    [SerializeField] GameObject bullet;
    [Range(0, 5)][SerializeField] float shootRate;
    [SerializeField] int scrapCost;
    [Range(0, 5)][SerializeField] int turretRotateSpeed;
    [SerializeField] Sprite turretIcon;

    Color startColor = Color.white;
    bool isShooting;
    int currentCannon;

    Transform target;
    List<Transform> targets = new List<Transform>(); // Holds our list of targets that came into our attack range.

    public int GetTurretCost() {
        return scrapCost;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCannon = 0;
        startColor = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null) { // If we have a valid target, go ahead and start shooting at it.
            Vector3 targetDir = target.position - turretHead.transform.position;
            Quaternion rot = Quaternion.LookRotation(new Vector3(targetDir.x, transform.position.y, targetDir.z));
            turretHead.transform.rotation = Quaternion.Lerp(turretHead.transform.rotation, rot, Time.deltaTime * turretRotateSpeed);

            if (!isShooting) {
                StartCoroutine(FireCannon());
            }
        }
        else { // Otherwise check if we have more targets that came into range and switch to them
            if(targets.Count > 0) {
                if(targets.First() == null) {
                    targets.RemoveAt(0);
                }
                if (targets.Count > 0) { // We still have targets to select from
                    target = targets.First();
                }
                else { // Otherwise just empty target for now.
                    target = null;
                }
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

    public void takeDamage(float amount) {
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
        if (other.CompareTag("Enemy")) {
            if(target == null) // If no target currently, go ahead and set target to this enemy.
                target = other.GetComponent<Transform>();

            // Add to our targets list.
            targets.Add(other.GetComponent<Transform>());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.isTrigger) {
            return;
        }
        if (other.CompareTag("Enemy")) { // check if this entity is an enemy,
            if (targets.Contains(other.GetComponent<Transform>())) { // if it is check if its in our list
                targets.Remove(other.GetComponent<Transform>()); // if so, remove it from our target list as its now out of range.
            }
        }
    }
}
