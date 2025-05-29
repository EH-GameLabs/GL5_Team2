using UnityEngine;

public class CM_Tentatore : CollectorMask
{
    public int cardAmount = 1;
    public override void ActivateMaskEffect()
    {
        GameManager.Instance.PlayerLife -= cardAmount;
    }
}
