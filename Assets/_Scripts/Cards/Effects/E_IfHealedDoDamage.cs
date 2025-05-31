using UnityEngine;

[CreateAssetMenu(fileName = "New IfHealedDoDamage", menuName = "ScriptableObject/Effects/IfHealedDoDamage")]
public class E_IfHealedDoDamage : SO_Effect
{
    [SerializeField] int dmg;
    public override void Effect()
    {
        TurnManager.Instance.SetHealCheck(dmg);
    }
}
