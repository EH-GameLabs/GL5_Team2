using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : BaseUI
{
    [Header("Health Info")]
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Slider enemyHealthSlider;

    [SerializeField] private TextMeshProUGUI maskText;

    [SerializeField] private TextMeshProUGUI playerRMText;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UIManager.instance.ShowUI(UIManager.GameUI.Pause);
            Time.timeScale = 0f; // Pause the game
            SoundManager.Instance.PauseMusic();
        }
    }

    public void UpdateRM(int rm)
    {
        playerRMText.text = "PlayerRM: " + rm;
    }

    public void UpdatePlayerHealth(float health, float maxHealth)
    {
        playerHealthSlider.value = health / maxHealth;
        if (playerHealthSlider.value <= 0)
        {
            UIManager.instance.ShowUI(UIManager.GameUI.Lose);
        }
    }

    public void UpdateEnemyHealth(float health, float maxHealth)
    {
        enemyHealthSlider.value = health / maxHealth;
        if (enemyHealthSlider.value <= 0)
        {
            UIManager.instance.ShowUI(UIManager.GameUI.Win);
        }
    }

    public void UpdateMaskText(string maskName)
    {
        maskText.text = "Mask:\n" + maskName;
    }
}
