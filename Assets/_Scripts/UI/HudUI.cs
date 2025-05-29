using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI playerRMText;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UIManager.instance.ShowUI(UIManager.GameUI.Pause);
        }
    }

    public void UpdateRM(int rm)
    {
        playerRMText.text = "PlayerRM: " + rm;
    }
}
