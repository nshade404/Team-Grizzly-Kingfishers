
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public List<GameObject> pickups;
    private Dictionary<GameObject, bool> pickupStates = new Dictionary<GameObject, bool>();
    private bool hasKey = false;
    private bool hasRocketPiece = false;
    public playerController playerController; // Reference to the PlayerController script

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
        pickup.SetActive(false);
        pickupStates[pickup] = true;

        if (pickup.CompareTag("Key"))
        {
            playerController.keysCollected++; // Increment key count in the PlayerController
            hasKey = true;
        }
        else if (pickup.CompareTag("HealthPickup"))
        {
            // Handle health pickup
        }
        else if (pickup.CompareTag("RocketPiece"))
        {
            Debug.Log("Player picked up a rocket piece!");
            hasRocketPiece = true;
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


