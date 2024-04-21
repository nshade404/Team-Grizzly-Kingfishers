using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Poison", fileName = "Poison")]
public class Poison : BaseEffect
{
    [SerializeField] float modifiedDOT;
    [SerializeField] bool isDOT = true;
    float dotTimer = 1f;
    [SerializeField] float modifiedSpeed = .9f;

    public override float Effect_Speed(float originalSpeed)
    {
        return originalSpeed * modifiedSpeed;
    }

    public override float Effect_DOT(float originalDMG)
    {
        if (isDOT)
        {
            dotTimer -= Time.deltaTime;
            if (dotTimer <= 0f)
            {
               
                modifiedDOT = originalDMG / Duration;
                dotTimer = 1f; // Reset timer for next DOT application
            }
        }
        return modifiedDOT;
    }
}
