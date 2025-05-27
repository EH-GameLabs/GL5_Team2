using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TakeDamage", menuName = "ScriptableObject/TakeDamage")]
public class E_DrawCards : SO_Effect
{
    Dictionary<int, CardTypes> m_Cards;
    public override void Effect()
    {
        foreach (var card in m_Cards)
        {
            GameManager.Instance.DrawCard(card.Value, card.Key);
        }
    }
}