using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public GameObject keyObject; 
    public Transform player; 

    private bool hasKey = false; 

    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && !hasKey)
        {
            
            keyObject.SetActive(false);

            
            hasKey = true;

            
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

