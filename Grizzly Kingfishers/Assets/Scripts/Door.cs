using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject keyPickup; // Reference to the key pickup GameObject

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if (playerController != null && playerController.HasKey())
            if (gameManager.instance.playerScript.HasKey())
            {
                OpenDoor();
                RemoveKey();
                gameManager.instance.playerScript.keysCollected--;
            }
            else
            {
                Debug.Log("Player does not have a key!");
                // Optionally, you can add code here to inform the player that they need a key to open the door
            }
        }
    }

    private void OpenDoor()
    {
        Debug.Log("Door opened!");
        Destroy(gameObject);
    }

    private void RemoveKey()
    {
        if (keyPickup != null)
        {
            keyPickup.SetActive(false);
        }
        

        // Alternatively, you might want to destroy the key pickup GameObject if you don't need it anymore
        // Destroy(keyPickup);
    }
}
