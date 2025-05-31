using UnityEngine;

[CreateAssetMenu(fileName = "New AlterRM", menuName = "ScriptableObject/Effects/AlterRM")]
public class E_AlterRMCost : SO_Effect
{
    [SerializeField] private int costChangeAmount; // Positive to increase cost, negative to decrease
    public override void Effect()
    {
        TurnManager.Instance.altereted += costChangeAmount;
    }
}
