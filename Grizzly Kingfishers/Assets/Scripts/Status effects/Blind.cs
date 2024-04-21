using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Blind", fileName = "Blind")]
public class Blind : BaseEffect
{
    [SerializeField] float modifiedFireRate = 0f;
    
    public override float Effect_Blind(float originalFireRate)
    {
        return originalFireRate * modifiedFireRate;
    }
}
