using UnityEngine;

[CreateAssetMenu(fileName = "New DoDamage", menuName = "ScriptableObject/Effects/DoDamage")]
public class E_DoDamage : SO_Effect
{
    [SerializeField] private int damageAmount;
    public override void Effect()
    {
        GameManager.Instance.EnemyLife -= damageAmount;
        FindAnyObjectByType<CM_Accusatore>().damageAmount = damageAmount;
    }
}
