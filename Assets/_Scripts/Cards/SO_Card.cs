using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObject/Card")]
public class SO_Card : ScriptableObject
{
    public string cardName;
    public CardTypes cardType;
    public int RMCost;
    public int lifeTime = 1;
    public List<SO_Effect> effects;
}

public enum CardTypes
{
    Echo, Lucido, Doloroso, Repressivo,
}
