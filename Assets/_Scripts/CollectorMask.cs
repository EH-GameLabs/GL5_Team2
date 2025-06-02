using System;
using UnityEngine;

public abstract class CollectorMask : MonoBehaviour
{
    public int currentCardInHand = 0;

    public void AddCard() { currentCardInHand += 1; }
    public void ResetCard() { currentCardInHand = 0; }
    public int GetCards() { return currentCardInHand; }

    public abstract void ActivateMaskEffect();
}
