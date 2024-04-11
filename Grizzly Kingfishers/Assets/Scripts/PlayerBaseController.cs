using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;

    [Header("----- Turret Stats -----")]
    [Range(0, 50)][SerializeField] float health;

    Color startColor = Color.white;

    private void Start() {
        startColor = model.material.color;
    }

    public void takeDamage(float amount) {
        health -= amount;
        StartCoroutine(flashRed());
        if (health <= 0) { 
            gameManager.instance.youHaveLost();
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed() {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = startColor;
    }
}
