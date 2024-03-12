using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTests : MonoBehaviour
{
    enum Bullets
    {
        None,
        Poison,
        Fire,
        Ice,
        Stone,
        Electric,
        PocketSand,
        visciousMockery
    }
    struct Bullet
    {
        public Bullets type;
        public string name;
        public int currentCount;
    }

    Bullet[] ammo;
    [SerializeField]int currentSlot;
    [SerializeField] Bullets bulletChoice;
    [SerializeField] int currentAmmo;
    
    bool isSwapping = false;
    string ammoName;

    // Start is called before the first frame update
    void Start()
    {
        //inventory check for bullet types <----
        Bullet Fire;
        Fire.type = Bullets.Fire;
        Fire.name = "Fire";
        Fire.currentCount = 0;

        Bullet Poison;
        Poison.type = Bullets.Poison;
        Poison.name = "Poison";
        Poison.currentCount = 0;

        Bullet Stone;
        Stone.type = Bullets.Stone;
        Stone.name = "Stone";
        Stone.currentCount = 0;

        Bullet Electric;
        Electric.type = Bullets.Electric;
        Electric.name = "Electric";
        Electric.currentCount = 0;

        Bullet Sand;
        Sand.type = Bullets.PocketSand;
        Sand.name = "Pocket Sand";
        Sand.currentCount = 0;

        Bullet Ice;
        Ice.type = Bullets.Ice;
        Ice.name = "Ice";
        Ice.currentCount = 0;

        Bullet Fun;
        Fun.type = Bullets.visciousMockery;
        Fun.name = "Insults";
        Fun.currentCount = 0;

        Bullet None;
        None.type = Bullets.None;
        None.name = "----";
        None.currentCount = 0;

        ammo[0] = None;
        ammo[1] = Poison;
        ammo[2] = Fire;
        ammo[3] = Ice;
        ammo[4] = Stone;
        ammo[5] = Electric;
        ammo[6] = Sand;
        ammo[7] = Fun;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1) && !isSwapping)
        {
            StartCoroutine(Swap());
        }
        Debug.Log(ammo[currentSlot].name.ToString());
    }
    IEnumerator Swap()
    {
        isSwapping = true;
        currentSlot++;
        if (currentSlot >= ammo.Length)
        {
            currentSlot = 1;
        }
        if (ammo[currentSlot].currentCount != 0)
        {
            bulletChoice = ammo[currentSlot].type;
            currentAmmo = ammo[currentSlot].currentCount;
            ammoName = ammo[currentSlot].name;
        }
        else
        {
            currentSlot = 0;
        }

        yield return new WaitForSeconds(.1f);
            isSwapping = false;
        }
    }
