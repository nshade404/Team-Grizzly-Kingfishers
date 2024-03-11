using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject == gameManager.instance.player) {
            GetComponentInParent<EnemyAI>().SetIsAggroing(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject == gameManager.instance.player) {
            GetComponentInParent<EnemyAI>().SetIsAggroing(false);
        }
    }
}
