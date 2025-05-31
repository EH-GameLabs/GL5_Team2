using UnityEngine;

[CreateAssetMenu(fileName = "New Selection", menuName = "ScriptableObject/Effects/Selection")]
public class E_Selection : SO_Effect
{
    public override void Effect()
    {
        PointerManager.Instance.selecting = true;
    }
}
