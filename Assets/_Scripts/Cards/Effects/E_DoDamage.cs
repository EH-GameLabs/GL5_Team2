using UnityEngine;

[CreateAssetMenu(fileName = "New DoDamage", menuName = "ScriptableObject/Effects/DoDamage")]
public class E_DoDamage : SO_Effect
{
    public int damageAmount;
    public override void Effect()
    {
        GameManager.Instance.EnemyLife -= damageAmount;
    }
}
