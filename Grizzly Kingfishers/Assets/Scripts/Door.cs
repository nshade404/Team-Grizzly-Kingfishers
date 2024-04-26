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
                //OpenDoor();

                StartCoroutine(doorAnimation());
                RemoveKey();
                gameManager.instance.playerScript.keysCollected--;
            }
            else
            {
                Debug.Log("Player does not have a key!");
               
            }
        }
    }

    /*private void OpenDoor()
    {
        Debug.Log("Door opened!");
        aud.PlayOneShot(audOpen, audOpenVol);
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        Destroy(gameObject);
    }
    */
    private void RemoveKey()
    {
        if (keyPickup != null)
        {
            keyPickup.SetActive(false);
        }
        

      
    }

    IEnumerator doorAnimation()
    {
        aud.PlayOneShot(audOpen, audOpenVol);
        gameObject.GetComponent<Collider>().enabled = false;
        Vector3 targetPosition = transform.position - new Vector3(0, -4f, 0);
        float lerpTime = 0f;
        float lerpDuration = 2f;
        Vector3 initialPosition = transform.position;
        while (lerpTime < lerpDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, lerpTime / lerpDuration);

            lerpTime += Time.deltaTime;

            yield return null;
        }
        Destroy(gameObject);
    }
}
