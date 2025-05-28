using UnityEngine;

[CreateAssetMenu(fileName = "E_Foreach_Do", menuName = "ScriptableObject/Effects/E_Foreach_Do")]
public class E_Foreach_Do : SO_Effect
{
    [SerializeField] private CardTypes cardTypeToCheck;
    [SerializeField] private SO_Effect effectToActivate;

    public override void Effect()
    {
        foreach (var slot in TurnManager.Instance.cardSlot)
        {
            Card card = slot.GetComponentInChildren<Card>();
            if (card == null) continue;

            if (card.cardData.cardType == cardTypeToCheck)
            {
                effectToActivate.Effect();
            }
        }
    }
}
