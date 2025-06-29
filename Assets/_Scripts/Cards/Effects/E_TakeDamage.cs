using UnityEngine;

[CreateAssetMenu(fileName = "New TakeDamage", menuName = "ScriptableObject/Effects/TakeDamage")]
public class E_TakeDamage : SO_Effect
{
    [SerializeField] private int damageAmount;
    public override void Effect()
    {
        GameManager.Instance.PlayerLife -= damageAmount;
    }
}