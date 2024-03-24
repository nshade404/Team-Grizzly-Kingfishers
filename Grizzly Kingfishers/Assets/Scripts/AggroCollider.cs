using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AggroCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("Turret")) {
            if(GetComponentInParent<EnemyAI>()) {
                GetComponentInParent<EnemyAI>().playerInRange = true;
                GetComponentInParent<EnemyAI>().target = other.GetComponent<Transform>();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("Turret")) {
            if (GetComponentInParent<EnemyAI>()) {
                GetComponentInParent<EnemyAI>().playerInRange = false;
            }
        }
    }
}
