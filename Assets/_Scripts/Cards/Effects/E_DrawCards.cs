using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DrawCards", menuName = "ScriptableObject/Effects/DrawCards")]
public class E_DrawCards : SO_Effect
{
    [SerializeField] private List<CardDraw> m_Cards = new();
    public override void Effect()
    {
        int i = 0;
        foreach (var card in m_Cards)
        {
            GameManager.Instance.DrawCard(card.cardType, card.amount);
            i += card.amount;
        }

        FindAnyObjectByType<CM_Tentatore>().cardAmount = i;
    }
}

[System.Serializable]
public class CardDraw
{
    public int amount;
    public CardTypes cardType;
}