using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObject/Card")]
public class SO_Card : ScriptableObject
{
    public string cardName;
    public CardTypes cardType;
    public int RMCost;
}

public enum CardTypes
{
    Echo, Lucido, Doloroso, Repressivo,
}
