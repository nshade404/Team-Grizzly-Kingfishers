using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectableObjects : MonoBehaviour
{
    List<BaseEffect> activeEffects = new List<BaseEffect>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //tick all active effects
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].TickEffect();

            if (!activeEffects[i].isActive)
            {
                activeEffects.RemoveAt(i);
            }
        }
    }

    public void ApplyEffect(BaseEffect effectTemplate)
    {
        //create a new effect instance
        var newEffect = ScriptableObject.Instantiate(effectTemplate);

        //activate effect
        newEffect.EneableEffect();
        activeEffects.Add(newEffect);
    }

    public float Effect_Speed(float originalSpeed)
    {
        float workingSpeed = originalSpeed;
        for(int i = 0; i < activeEffects.Count; i++)
        {
            if (!activeEffects[i].isActive)
            {
                continue;
            }
            workingSpeed = activeEffects[i].Effect_Speed(workingSpeed);
        }
        return workingSpeed;
    }

    public float Effect_Jumps(float originalJumps)
    {
        float workingJumps = originalJumps;
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (!activeEffects[i].isActive)
            {
                continue;
            }
            workingJumps = activeEffects[i].Effect_Speed(workingJumps);
        }
        return workingJumps;
    }

    public float Effect_Blind(float originalFireRate)
    {
        float workingFireRate = originalFireRate;
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (!activeEffects[i].isActive)
            {
                continue;
            }
            workingFireRate = activeEffects[i].Effect_Speed(workingFireRate);
        }
        return workingFireRate;
    }
}
