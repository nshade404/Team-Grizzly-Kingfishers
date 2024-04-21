using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTrigger : MonoBehaviour
{
    [SerializeField] BaseEffect Effect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        var effectable = other.gameObject.GetComponent<EffectableObjects>();
        if (effectable != null)
        {
            effectable.ApplyEffect(Effect);
        }
    }
}
