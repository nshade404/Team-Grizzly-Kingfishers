using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Slow", fileName = "Slow")]
public class Slow : BaseEffect
{
    [SerializeField] float modifiedSpeed = 0.8f;
    public override float Effect_Speed(float originalSpeed)
    {
        return originalSpeed * modifiedSpeed;
    }
}
