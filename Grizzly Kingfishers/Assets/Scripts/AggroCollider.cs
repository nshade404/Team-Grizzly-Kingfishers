using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AggroCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.isTrigger) {
            return;
        }
        if (other.CompareTag("Player") || other.CompareTag("Turret") || other.CompareTag("PlayerBase")) {
            if(GetComponentInParent<EnemyAI>()) {
                GetComponentInParent<EnemyAI>().playerInRange = true;
                GetComponentInParent<EnemyAI>().target = other.GetComponent<Transform>();
                if (GetComponentInParent<EnemyAI>().target == null) // If no target currently, go ahead and set target to this enemy.
                    GetComponentInParent<EnemyAI>().target = other.GetComponent<Transform>();

                // Add to our targets list.
                GetComponentInParent<EnemyAI>().targets.Add(other.GetComponent<Transform>());
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("Turret") || other.CompareTag("PlayerBase")) {
            if (GetComponentInParent<EnemyAI>()) {
                GetComponentInParent<EnemyAI>().playerInRange = false;
                if (GetComponentInParent<EnemyAI>().targets.Contains(other.GetComponent<Transform>())) { // if it is check if its in our list
                    GetComponentInParent<EnemyAI>().targets.Remove(other.GetComponent<Transform>()); // if so, remove it from our target list as its now out of range.
                }
            }
        }
    }
}
