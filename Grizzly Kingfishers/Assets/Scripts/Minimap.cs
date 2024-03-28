using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private gameManager gameManagerInstance; 

    private void Awake()
    {
        gameManagerInstance = gameManager.instance; 
    }

    private void LateUpdate()
    {
        if (gameManagerInstance != null && gameManagerInstance.player != null)
        {
            Transform player = gameManagerInstance.player.transform;

            // Update minimap position
            Vector3 newPos = player.position;
            newPos.y = transform.position.y;
            transform.position = newPos;

            // Update minimap rotation
            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }
        
    }
}


