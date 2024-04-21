using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpike : MonoBehaviour
{
    // just base damage but we can balance later on
    public float damageAmount = 5f; 
    public float extendSpeed = 5f; 
    public float retractSpeed = 2f;
    public float extendedHeight = 2f;
    public float retractedHeight = 0f;

    private List<playerController> listPlayers = new List<playerController>();
    private bool extending = false;
    private bool retracted = false;

    private void Start()
    {
         listPlayers.Clear();
    }

    private void Update()
    {

        if (listPlayers.Count > 0 && !extending && !retracted)
        {
            StartCoroutine(ExtendSpikes());
        }
        else if (listPlayers.Count == 0 && !extending && !retracted&& transform.position.y > retractedHeight)
        {
            
            StartCoroutine(RetractSpikes());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerController player = other.gameObject.GetComponent<playerController>();

        if (player != null)
        {
            if (!listPlayers.Contains(player))
            {
                listPlayers.Add(player);
                if (!extending && !retracted)
                {
                    StartCoroutine(ExtendSpikes());
                }
            }
        }
    }
    // both enter and exit check if the player is in the list so it knows to do damage and also extend and retract
    private void OnTriggerExit(Collider other)
    {
        playerController player = other.gameObject.GetComponent<playerController>();

        if (player != null)
        {
            if (listPlayers.Contains(player))
            {
                listPlayers.Remove(player);
                if (listPlayers.Count == 0 && !extending && !retracted && transform.position.y > retractedHeight)
                {
                    StartCoroutine(RetractSpikes());
                }
            }
        }
    }


    private IEnumerator ExtendSpikes()
    {
        extending = true;
        while (transform.position.y < extendedHeight)
        {
            transform.Translate(Vector3.up * extendSpeed * Time.deltaTime);
            yield return null;
        }
        extending = false;

       
        foreach (playerController player in listPlayers)
        {
            DamagePlayer(player);
        }
    }

    
    private IEnumerator RetractSpikes()
    {

        while (transform.position.y > retractedHeight)
        {
            transform.Translate(Vector3.down * retractSpeed * Time.deltaTime);
            yield return null;
        }
        extending = true;
    }
    // was having trouble getting this to trigger along with damaging the player
    
   private void DamagePlayer(playerController player)
    {
        IDamage damageable = player.GetComponent<IDamage>();
        if (damageable != null)
        {
            damageable.takeDamage(damageAmount);
        }
    }
}
