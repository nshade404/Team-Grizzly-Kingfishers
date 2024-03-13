using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public enum DamageType {
        Normal = 0,
        Poison,
        Fire,
        Ice,
        Stone,
        Electric,
        PocketSand,
        visciousMockery
    }

    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] DamageType type;

    public DamageType GetDamageType() {
        return type;
    }

    // Start is called before the first frame update
    void Start() {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.isTrigger) {
            return;
        }

        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null) {
            dmg.takeDamage(damage);
        }

        Destroy(gameObject);
    }
}
