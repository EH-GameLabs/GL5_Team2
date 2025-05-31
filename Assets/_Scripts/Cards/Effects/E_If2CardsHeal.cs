using UnityEngine;

[CreateAssetMenu(fileName = "New If2CardsHeal", menuName = "ScriptableObject/Effects/If2CardsHeal")]
public class E_If2CardsHeal : SO_Effect
{
    public override void Effect()
    {
        TurnManager.Instance.SetCardsCheck();
    }
}
