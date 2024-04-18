
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public List<GameObject> pickups;
    private Dictionary<GameObject, bool> pickupStates = new Dictionary<GameObject, bool>();
    private bool hasKey = false;
    private bool hasRocketPiece = false;

    private void Start()
    {
        foreach (GameObject pickup in pickups)
        {
            pickupStates[pickup] = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject pickup in pickups)
            {
                if (pickup.activeSelf && !pickupStates[pickup])
                {
                    PickupItem(pickup);
                }
            }
        }
    }

    private void PickupItem(GameObject pickup)
    {
        pickupStates[pickup] = true;

        if (pickup.CompareTag("Key"))
        {
            gameManager.instance.playerScript.keysCollected++; 
            hasKey = true;
            pickup.SetActive(false);
        }
        else if (pickup.CompareTag("HealthPickup"))
        {
            pickup.SetActive(false);

        }
        else if (pickup.CompareTag("RocketPiece"))
        {
            Debug.Log("Player picked up a rocket piece!");
            hasRocketPiece = true;
        }

        else if (pickup.CompareTag("AmmoPickup")) 
        {
            pickup.SetActive(false);
            
        }
    }

    public bool HasKey()
    {
        return hasKey;
    }

    public bool HasRocketPiece()
    {
        return hasRocketPiece;
    }
}


