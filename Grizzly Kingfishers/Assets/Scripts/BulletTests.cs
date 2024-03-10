using System.Collections;
using System.Collections.Generic;
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
        PocketSand
    }

    [SerializeField] Bullets bulletChoice;
    [SerializeField] int currentAmmo;
    [SerializeField] int numPoison;
    [SerializeField] int numFire;
    [SerializeField] int numIce;
    [SerializeField] int numStone;
    [SerializeField] int numElectric;
    [SerializeField] int numSand;
    bool isSwapping = false;
    string ammoName;
    // Start is called before the first frame update
    void Start()
    {
        //inventory check for bullet types <----
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1) && !isSwapping)
        {
            StartCoroutine(Swap());
            Debug.Log(ammoName);
        }

    }
    IEnumerator Swap()
    {
        isSwapping = true;
        switch (bulletChoice)
        {
            case Bullets.None:
                if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                    currentAmmo = numPoison;
                    ammoName = "Posion";
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                    currentAmmo = numFire;
                    ammoName = "Fire";
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                    currentAmmo = numIce;
                    ammoName = "Ice";
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                    currentAmmo = numStone;
                    ammoName = "Stone";
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                    currentAmmo = numElectric;
                    ammoName = "Electric";
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                    currentAmmo = numSand;
                    ammoName = "Sand";
                }
                else
                {
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                    ammoName = "----";
                }
                break;
            case Bullets.Poison:
                if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                    currentAmmo = numFire;
                    ammoName = "Fire";
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                    currentAmmo = numIce;
                    ammoName = "Ice";
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                    currentAmmo = numStone;
                    ammoName = "Stone";
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                    currentAmmo = numElectric;
                    ammoName = "Electric";
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                    currentAmmo = numSand;
                    ammoName = "Sand";
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                    currentAmmo = numPoison;
                    ammoName = "Poison";
                }
                else
                {
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                    ammoName = "----";
                }

                break;
            case Bullets.Fire:

                if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                    currentAmmo = numIce;
                    ammoName = "Ice";
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                    currentAmmo = numStone;
                    ammoName = "Stone";
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                    currentAmmo = numElectric;
                    ammoName = "Electric";
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                    currentAmmo = numSand;
                    ammoName = "Sand";
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                    currentAmmo = numPoison;
                    ammoName = "Poison";
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                    currentAmmo = numFire;
                    ammoName = "Fire";
                }
                else
                {
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                    ammoName = "----";
                }

                break;
            case Bullets.Ice:

                if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                    currentAmmo = numStone;
                    ammoName = "Stone";
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                    currentAmmo = numElectric;
                    ammoName = "Electric";
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                    currentAmmo = numSand;
                    ammoName = "Sand";
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                    currentAmmo = numPoison;
                    ammoName = "Poison";
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                    currentAmmo = numFire;
                    ammoName = "Fire";
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                    currentAmmo = numIce;
                    ammoName = "Ice";
                }
                else
                {
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                    ammoName = "----";
                }
                break;
            case Bullets.Stone:

                if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                    currentAmmo = numElectric;
                    ammoName = "Electric";
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                    currentAmmo = numSand;
                    ammoName = "Sand";
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                    currentAmmo = numPoison;
                    ammoName = "Poison";
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                    currentAmmo = numFire;
                    ammoName = "Fire";
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                    currentAmmo = numIce;
                    ammoName = "Ice";
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                    currentAmmo = numStone;
                    ammoName = "Stone";
                }
                else
                {
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                    ammoName = "----";
                }

                break;
            case Bullets.Electric:

                if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                    currentAmmo = numSand;
                    ammoName = "Sand";
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                    currentAmmo = numPoison;
                    ammoName = "Poison";
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                    currentAmmo = numFire;
                    ammoName = "Fire";
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                    currentAmmo = numIce;
                    ammoName = "Ice";
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                    currentAmmo = numStone;
                    ammoName = "Stone";
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                    currentAmmo = numElectric;
                    ammoName = "Electric";
                }
                else
                {
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                    ammoName = "----";
                }

                break;
            case Bullets.PocketSand:

                if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                    currentAmmo = numPoison;
                    ammoName = "Poison";
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                    currentAmmo = numFire;
                    ammoName = "Fire";
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                    currentAmmo = numIce;
                    ammoName = "Ice";
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                    currentAmmo = numStone;
                    ammoName = "Stone";
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                    currentAmmo = numElectric;
                    ammoName = "Electric";
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                    currentAmmo = numSand;
                    ammoName = "Sand";
                }
                else
                {
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                    ammoName = "----";
                }
                
                break;

            }
        
        yield return new WaitForSeconds(.1f);
            isSwapping = false;
        }
    }
