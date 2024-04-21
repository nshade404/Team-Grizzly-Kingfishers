using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : ScriptableObject
{
    [SerializeField] protected float Duration = 0f;
    float DurationRemaining = 0f;
    public bool isActive => DurationRemaining > 0f;

    public virtual void EneableEffect()
    {
        DurationRemaining = Duration;
    }

    public virtual void TickEffect()
    {
        if (DurationRemaining > 0f) 
        {
            DurationRemaining -= Time.deltaTime;
        }
    }

    public virtual void TickDOT()
    {

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
