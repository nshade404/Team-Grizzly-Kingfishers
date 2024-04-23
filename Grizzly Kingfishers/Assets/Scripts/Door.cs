using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject keyPickup; // Reference to the key pickup GameObject
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audOpen;
    [Range(0, 1)][SerializeField] float audOpenVol;

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
               
            }
        }
    }

    private void OpenDoor()
    {
        Debug.Log("Door opened!");
        aud.PlayOneShot(audOpen, audOpenVol);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject);
    }

    private void RemoveKey()
    {
        if (keyPickup != null)
        {
            keyPickup.SetActive(false);
        }
        

      
    }
}
