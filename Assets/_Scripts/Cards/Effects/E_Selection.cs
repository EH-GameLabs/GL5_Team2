using UnityEngine;

[CreateAssetMenu(fileName = "New Selection", menuName = "ScriptableObject/Effects/Selection")]
public class E_Selection : SO_Effect
{
    public override void Effect()
    {
        if (GameManager.Instance.GetNCards() == 0) return;

        PointerManager.Instance.selecting = true;
        Time.timeScale = 0f; // Pause the game
        Debug.Log("Select a card in ur hand");
        FindAnyObjectByType<HudUI>(FindObjectsInactive.Include).discardACard.SetActive(true);
    }
}
