using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Freeze", fileName = "Freeze")]
public class Freeze : BaseEffect
{

    public override float Effect_Speed(float originalSpeed)
    {
        return originalSpeed;
    }

    public override float Effect_Jumps(float originalJumps)
    {
        return originalJumps;
    }

    public override float Effect_Blind(float originalFireRate)
    {
        return originalFireRate;
    }
}

