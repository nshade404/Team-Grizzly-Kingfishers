using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTests : MonoBehaviour
{
    enum Bullets
    {
        Poison,
        Fire,
        Ice,
        Stone,
        Electric,
        PocketSand
    }

    [SerializeField] Bullets bulletChoice;
    bool isSwapping = false;
    // Start is called before the first frame update
    void Start()
    {
        
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
                case Bullets.Poison:
                    bulletChoice = Bullets.Fire;
                    break;
                case Bullets.Fire:
                    bulletChoice = Bullets.Ice;
                    break;
                case Bullets.Ice:
                    bulletChoice = Bullets.Stone;
                    break;
                case Bullets.Stone:
                    bulletChoice = Bullets.Electric;
                    break;
                case Bullets.Electric:
                    bulletChoice = Bullets.PocketSand;
                    break;
                case Bullets.PocketSand:
                    bulletChoice = Bullets.Poison;
                    break;

            }
            yield return new WaitForSeconds(2);
            isSwapping = false;
        }
    }
