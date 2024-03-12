using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    enum DamageType {
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

    // Start is called before the first frame update
    void Start() {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
        SetColorByType();
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

    private void SetColorByType() {
        Color color = Color.white;
        switch (type) {
            case DamageType.Normal:
                color = Color.white;
                break;
            case DamageType.Poison:
                color = Color.green;
                break;
            case DamageType.Fire:
                color = Color.red;
                break;
            case DamageType.Ice:
                color = Color.blue;
                break;
            case DamageType.Stone:
                color = Color.gray;
                break;
            case DamageType.Electric:
                color = Color.cyan;
                break;
            case DamageType.PocketSand:
                color = Color.yellow;
                break;
            case DamageType.visciousMockery:
                color = Color.magenta;
                break;
        }

        GetComponent<TrailRenderer>().startColor = color;
    }
}
