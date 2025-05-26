using UnityEngine;

public abstract class SO_Effect : ScriptableObject
{
    [SerializeField] private string effectName;

    public virtual void Effect() { }
}
