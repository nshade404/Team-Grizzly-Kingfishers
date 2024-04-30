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
    [SerializeField] EffectableObjects Effectable;
    [SerializeField] AudioSource aud;
    [Header("----- Turret Stats -----")]
    [SerializeField] string displayName;
    [Range(0, 50)][SerializeField] float health;
    [SerializeField] Transform[] cannonBarrels;
    [SerializeField] GameObject bullet;
    [Range(0, 5)][SerializeField] float shootRate;
    [SerializeField] int scrapCost;
    [Range(0, 5)][SerializeField] int turretRotateSpeed;
    public Sprite turretIcon;

    [Header("----- Detection Settings -----")]
    [SerializeField] int viewCone; // our field of view


    Color startColor = Color.white;
    bool isShooting;
    int currentCannon;

    Transform target;
    List<Transform> targets = new List<Transform>(); // Holds our list of targets that came into our attack range.
    [SerializeField] AudioClip audShoot;
    [Range(0, 1)][SerializeField] float audShootVol;
    public string GetDisplayName() {
        return displayName;
    }

    public Bullet GetBulletType() {
        return bullet.GetComponent<Bullet>();
    }

    public float GetShootRate() {
        return shootRate;
    }

    public int GetRotationSpeed() {
        return turretRotateSpeed;
    }

    public int GetTurretCost() {
        return scrapCost;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCannon = 0;
        startColor = model.material.color;
        Effectable = GetComponent<EffectableObjects>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null && target.GetComponent<EnemyAI>().IsAlive) { // If we have a valid target, go ahead and start shooting at it.

            int randPos = Random.Range(0, 5);
            Vector3 targetDir = (target.position + (target.transform.forward * randPos)) - turretHead.transform.position;
            Quaternion rot = Quaternion.LookRotation(new Vector3(targetDir.x, transform.position.y, targetDir.z));
            turretHead.transform.rotation = Quaternion.Lerp(turretHead.transform.rotation, rot, Time.deltaTime * turretRotateSpeed);

            // TODO: Josh, Wanted to get them to only shoot when they are within their 'viewcone' but don't have time to debug this more right now...
            //float angleToTarget = Vector3.Angle(targetDir, turretHead.transform.forward);
            //RaycastHit hit;
            //if(Physics.Raycast(turretHead.transform.position, targetDir, out hit)) {
            //    bool hitisTargetable = hit.collider.CompareTag("Enemy");
            //    if(hitisTargetable && angleToTarget <= viewCone) {
                    
            //    }
            //}

            if (!isShooting) {
                StartCoroutine(FireCannon());
            }

        }
        else { // Otherwise check if we have more targets that came into range and switch to them
            if(targets.Count > 0) {
                if(targets.First() == null || !targets.First().GetComponent<EnemyAI>().IsAlive) {
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
        aud.PlayOneShot(audShoot, audShootVol);
        currentCannon++; // increment to next barrel for next shot,
        if (currentCannon == cannonBarrels.Length) { // If we are at num barrels.length, cycle back to 0
            currentCannon = 0;
        }

        //yield return new WaitForSeconds(Effectable.Effect_Blind(shootRate));
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
