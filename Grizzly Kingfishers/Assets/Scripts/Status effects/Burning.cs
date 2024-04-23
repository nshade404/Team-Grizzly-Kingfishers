using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Burning", fileName = "Burning")]
public class Burning : BaseEffect
{
    [SerializeField] float modifiedDOT;
    [SerializeField] bool isDOT = true;

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