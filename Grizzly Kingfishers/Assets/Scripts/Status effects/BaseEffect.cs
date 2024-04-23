using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : ScriptableObject
{
    [SerializeField] protected float Duration = 0f;
    float DurationRemaining = 0f;
    bool hasDOT = false;
    public bool isActive => DurationRemaining > 0f;
    protected float dotTimer = 1f;

    public virtual void EnableEffect()
    {
        DurationRemaining = Duration;
    }

    public virtual void TickEffect()
    {
        if (DurationRemaining > 0f) 
        {
            DurationRemaining -= Time.deltaTime;
        }
        if(hasDOT) 
        {

        }
    }


    public virtual float Effect_Speed(float originalSpeed)
    {
        return originalSpeed;
    }

    public virtual float Effect_Jumps(float originalJumps)
    {
        return originalJumps;
    }

    public virtual float Effect_Blind(float originalFireRate)
    {
        return originalFireRate;
    }

    public virtual float Effect_DOT(float originalDMG)
    {
        return originalDMG;
    }
}
