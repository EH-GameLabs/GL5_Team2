using UnityEngine;

public class CM_Martire : CollectorMask
{
    public int lifeAmount = 1;
    public override void ActivateMaskEffect()
    {
        GameManager.Instance.PlayerLife -= lifeAmount * 2;
    }
}
