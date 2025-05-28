using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DisableCollectMask", menuName = "ScriptableObject/Effects/DisableCollectMask")]
public class E_DisableCollectMask : SO_Effect
{
    public override void Effect()
    {
        TurnManager.Instance.currentTurn.collectorCanActivateEffect = false;
    }
}
