using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public GameObject keyObject;
    public GameObject healthObject;
    public Transform player; 

    private bool hasKey = false;
    private bool hasHealth = false;

    
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !hasKey)
        {

            keyObject.SetActive(false);


            hasKey = true;
        }

        else if (gameObject == healthObject && !hasHealth)
        {
            healthObject.SetActive(false);
            
            
            hasHealth = true;
        }


    }
    

    
    public bool HasKey()
    {
        return hasKey;
    }

    
    public void UseKey()
    {
        
        if (hasKey)
        {
           
            Debug.Log("Using key to open a door...");

            
            hasKey = false;
        }
        else
        {
            Debug.Log("Player doesn't have the key!");
        }
    }
}

