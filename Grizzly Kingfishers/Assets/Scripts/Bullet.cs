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

    public float damage;
    public float speed;
    [SerializeField] int destroyTime;
    [SerializeField] DamageType type;

    [Header("----- Bullet Effects -----")]
    public float effTime;
    public float slowPerc;
    public bool blind;

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
        type = GetDamageType();

        switch ((int)type)
        {
            case 0:
                effTime = 0; 
                slowPerc = 0;
                blind = false;
                break;
            case 1:
                effTime = 2;
                slowPerc = 0.1f;
                blind = false;
                break;
            case 2: 
                effTime = 5;
                slowPerc = 0;
                blind = false;
                break;
            case 3:
                effTime = 2;
                slowPerc += .1f;
                blind = false; 
                break;
            case 4:
                effTime = 1;
                slowPerc = 1;
                blind = true;
                break;
            case 5:
                effTime = 1;
                slowPerc = 1;
                blind = true;
                break;
            case 6:
                effTime = 3;
                slowPerc = .2f;
                blind = true;
                break;


        }


        Destroy(gameObject);
    }
}
