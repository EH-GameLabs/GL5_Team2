using UnityEngine;

[CreateAssetMenu(fileName = "New GainLife", menuName = "ScriptableObject/Effects/GainLife")]
public class E_GainLife : SO_Effect
{
    [SerializeField] private int lifeAmount = 1;
    public override void Effect()
    {
        if (!(GameManager.Instance.playerMaxLife == GameManager.Instance.PlayerLife))
        {
            GameManager.Instance.PlayerLife += lifeAmount;
            TurnManager.Instance.SetHealed();
        }

        CM_Martire martire = FindAnyObjectByType<CM_Martire>();
        martire.lifeAmount = lifeAmount;
    }
}
