using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrets : MonoBehaviour
{
    [Range(1, 10)][SerializeField] int scrapCost;

    [Header("----- Turret Stats -----")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [Range(0, 5)][SerializeField] float shootRate;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bullet != null)
        {
            Bullet.DamageType type = bullet.GetComponent<Bullet>().GetDamageType();
            if (bullet.GetComponent<Bullet>().GetDamageType() == Bullet.DamageType.visciousMockery)
            {
                FireInsult();
            }
            else
            {
                Instantiate(bullet, shootPos.position, transform.rotation);
            }
        }
    }

    public void FireInsult()
    {
        int choice = Random.Range(0, 5);
        string saying = "";
        switch (choice)
        {
            case 0:
                saying = "You sure you don't need glasses?!";
                break;
            case 1:
                saying = "Grandma has more gun stability than you!";
                break;
            case 2:
                saying = "What're you? A Stormtrooper??";
                break;
            case 3:
                saying = "I think your brain is AFK.";
                break;
            case 4:
                saying = "Your father smelled of elderberries!";
                break;
        }

        Debug.Log(saying);
    }
}
