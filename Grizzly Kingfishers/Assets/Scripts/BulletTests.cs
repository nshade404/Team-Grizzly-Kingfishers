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
        Debug.Log(bulletChoice);
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
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                }
                else
                {
                    bulletChoice = Bullets.None;
                }
                break;
            case Bullets.Poison:
                    if(numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                }
                else if(numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                }
                else
                {
                    bulletChoice = Bullets.None;
                }
                break;
                case Bullets.Fire:
                
                if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                }
                else
                {
                    bulletChoice = Bullets.None;
                }
                break;
                case Bullets.Ice:
                
                if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                }
                else
                {
                    bulletChoice = Bullets.None;
                }
                break;
                case Bullets.Stone:
                
                if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                }
                else
                {
                    bulletChoice = Bullets.None;
                }
                break;
                case Bullets.Electric:
                
                if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                }
                else if (numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                }
                else
                {
                    bulletChoice = Bullets.None;
                }
                break;
                case Bullets.PocketSand:

                if(numPoison > 0)
                {
                    bulletChoice = Bullets.Poison;
                }
                else if (numFire > 0)
                {
                    bulletChoice = Bullets.Fire;
                }
                else if (numIce > 0)
                {
                    bulletChoice = Bullets.Ice;
                }
                else if (numStone > 0)
                {
                    bulletChoice = Bullets.Stone;
                }
                else if (numElectric > 0)
                {
                    bulletChoice = Bullets.Electric;
                }
                else if (numSand > 0)
                {
                    bulletChoice = Bullets.PocketSand;
                }
                else
                {
                    bulletChoice = Bullets.None;
                }
                break;
                default:
                    bulletChoice = Bullets.None;
                    break;

            }
            yield return new WaitForSeconds(.2f);
            isSwapping = false;
        }
    }
