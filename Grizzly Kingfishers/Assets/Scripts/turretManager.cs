using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class turretManager : MonoBehaviour
{
    [SerializeField] GameObject selectedTurret;
    [Range(1,10)][SerializeField] int timer;
    [SerializeField] Image progress;
    
    public bool isBuilding;

    // Start is called before the first frame update
    void Start()
    {
        selectedTurret = gameManager.instance.playerScript.selectedTurret;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Building());
    }

    IEnumerator Building()
    {
        isBuilding = true;
        float currentTime = 0;
        while(currentTime < timer)
        {
            currentTime += Time.deltaTime;
            progress.fillAmount = currentTime/timer;
        }
        yield return new WaitForSeconds(timer);
        Instantiate(selectedTurret, gameObject.transform.position, transform.rotation);
        Destroy(gameObject);
        isBuilding = false;
    }
}
