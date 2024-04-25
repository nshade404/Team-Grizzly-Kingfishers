using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LerpTowardsPlayer : MonoBehaviour
{
    public Transform player;
    public float floatSpeed = 5f;

    // Reference to the parent transform
    private Transform parentTransform;

    // Boolean flag to determine if lerping should occur
    private bool shouldLerp = false;

    void Start()
    {
        parentTransform = transform.parent;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldLerp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldLerp = false;
        }
    }

    void Update()
    {
        if (shouldLerp)
        {
            Transform playerPosition = gameManager.instance.player.transform;
            float step = floatSpeed * Time.deltaTime;
            parentTransform.position = Vector3.Lerp(parentTransform.position, playerPosition.position, step);
        }
    }
}
