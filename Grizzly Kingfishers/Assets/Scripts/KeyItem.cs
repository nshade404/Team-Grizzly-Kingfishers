using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "player")
        {
            GameVariables.keyCount += 2;
            Destroy(gameObject);
        }
        
    }
}
