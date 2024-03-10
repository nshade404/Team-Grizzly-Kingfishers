using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public GameObject Key;
    public GameObject keyistrue;
    public bool isPlayer;
    
    void Start()
    {
        isPlayer = false;

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayer = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            isPlayer= false;
        }
        
    }


    void Update()
    {
        if (isPlayer)
        {
                {
                    if(Input.GetKey(KeyCode.E)) 
                    {
                    Key.SetActive(true);
                    keyistrue.SetActive(true);
                    Destroy(gameObject);

                    }
                }
                
            
        }
        
    }
}
