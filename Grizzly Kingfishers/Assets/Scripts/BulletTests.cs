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
    // Start is called before the first frame update
    void Start()
    {
        //inventory check for bullet types <----
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2") && !isSwapping)
        {
            StartCoroutine(Swap());
        }
        Debug.Log(bulletChoice+ "/" +currentAmmo);
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
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                    currentAmmo = numFire;
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                    currentAmmo = numIce;
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                    currentAmmo = numStone;
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                    currentAmmo = numElectric;
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                    currentAmmo = numSand;
                }
                else
                {
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                }
                break;
            case Bullets.Poison:
                    if(numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                    currentAmmo = numFire;
                }
                else if(numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                    currentAmmo = numIce;
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                    currentAmmo = numStone;
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                    currentAmmo = numElectric;
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                    currentAmmo = numSand;
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                    currentAmmo = numPoison;
                }
                else
                {
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                }
                break;
                case Bullets.Fire:
                
                if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                    currentAmmo = numIce;
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                    currentAmmo = numStone;
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                    currentAmmo = numElectric;
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                    currentAmmo = numSand;
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                    currentAmmo = numPoison;
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                    currentAmmo = numFire;
                }
                else
                {
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                }
                break;
                case Bullets.Ice:
                
                if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                    currentAmmo = numStone;
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                    currentAmmo = numElectric;
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                    currentAmmo = numSand;
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                    currentAmmo = numPoison;
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                    currentAmmo = numFire;
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                    currentAmmo = numIce;
                }
                else
                {
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                }
                break;
                case Bullets.Stone:
                
                if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                    currentAmmo = numElectric;
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                    currentAmmo = numSand;
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                    currentAmmo = numPoison;
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                    currentAmmo = numFire;
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                    currentAmmo = numIce;
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                    currentAmmo = numStone;
                }
                else
                {
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                }
                break;
                case Bullets.Electric:
                
                if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                    currentAmmo = numSand;
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                    currentAmmo = numPoison;
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                    currentAmmo = numFire;
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                    currentAmmo = numIce;
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                    currentAmmo = numStone;
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                    currentAmmo = numElectric;
                }
                else
                {
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                }
                break;
                case Bullets.PocketSand:

                if(numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                    currentAmmo = numPoison;
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                    currentAmmo = numFire;
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                    currentAmmo = numIce;
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                    currentAmmo = numStone;
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                    currentAmmo = numElectric;
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                    currentAmmo = numSand;
                }
                else
                {
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                }
                break;
                default:
                    bulletChoice = Bullets.None;
                    currentAmmo = 0;
                    break;

            }
            yield return new WaitForSeconds(.2f);
            isSwapping = false;
        }
    }
