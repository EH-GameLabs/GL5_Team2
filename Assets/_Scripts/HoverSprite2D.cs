using UnityEngine;

public class HoverSprite2D : MonoBehaviour
{
    public GameObject spriteToShow;

    void OnMouseEnter()
    {
        if (UIManager.instance.GetCurrentActiveUI() != UIManager.GameUI.HUD) return;
        spriteToShow.SetActive(true);
    }

    void OnMouseExit()
    {
        spriteToShow.SetActive(false);
    }

    private void Update()
    {
        if (UIManager.instance.GetCurrentActiveUI() != UIManager.GameUI.HUD)
        {
            spriteToShow.SetActive(false);
        }
    }
}
