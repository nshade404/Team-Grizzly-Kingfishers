using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class turretManager : MonoBehaviour
{
    [SerializeField] GameObject selectedTurret;
    [Range(1,10)][SerializeField] int timer;
    [SerializeField] Image progress;
    
    public bool isBuilding;
    float currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        selectedTurret = gameManager.instance.playerScript.selectedTurret;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        progress.fillAmount = currentTime / timer;

        StartCoroutine(Building());
    }

    IEnumerator Building()
    {
        isBuilding = true;
        yield return new WaitForSeconds(timer);
        Instantiate(selectedTurret, gameObject.transform.position, transform.rotation);
        Destroy(gameObject);
        isBuilding = false;
    }
}
