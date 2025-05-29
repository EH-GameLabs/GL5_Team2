using System;
using UnityEngine;

public abstract class CollectorMask : MonoBehaviour
{
    public int currentCardActivated = 0;

    public void AddCardActivated() { currentCardActivated += 1; }

    public abstract void ActivateMaskEffect();
}
