using UnityEngine;

[CreateAssetMenu(fileName = "New GainLife", menuName = "ScriptableObject/Effects/GainLife")]
public class E_GainLife : SO_Effect
{
    [SerializeField] private int lifeAmount = 1;
    public override void Effect()
    {
        GameManager.Instance.PlayerLife += lifeAmount;

        CM_Martire martire = FindAnyObjectByType<CM_Martire>();
        martire.lifeAmount = lifeAmount;
        martire.AddCardActivated();
    }
}
