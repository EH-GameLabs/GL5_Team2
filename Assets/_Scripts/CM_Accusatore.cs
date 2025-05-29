using System;
using UnityEngine;

public class CM_Accusatore : CollectorMask
{
    public int damageAmount = 1;
    public override void ActivateMaskEffect()
    {
        GameManager.Instance.PlayerLife -= damageAmount;
    }
}
