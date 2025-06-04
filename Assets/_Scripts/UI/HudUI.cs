using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : BaseUI
{
    [Header("Health Info")]
    [SerializeField] private List<GameObject> playerHealths = new();
    [SerializeField] private Slider enemyHealthSlider;

    [SerializeField] private List<GameObject> playerRMText;

    [Header("Card Hover")]
    public Image cardHover;

    [Header("Player Info")]
    public GameObject discardACard;
    public GameObject LessRMCost;

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
        foreach (GameObject g in playerRMText)
        {
            g.SetActive(false);
        }
        playerRMText[rm].SetActive(true);
    }

    public void UpdatePlayerHealth(int health)
    {
        foreach (GameObject g in playerHealths)
        {
            g.SetActive(false);
        }
        playerHealths[health].SetActive(true);

        if (health <= 0)
        {
            StartCoroutine(LoseCoroutine());
        }
    }

    private IEnumerator LoseCoroutine()
    {
        yield return new WaitForSeconds(1f);
        UIManager.instance.ShowUI(UIManager.GameUI.Lose);
    }

    public void UpdateEnemyHealth(float health, float maxHealth)
    {
        enemyHealthSlider.value = health / maxHealth;
        if (enemyHealthSlider.value <= 0)
        {
            StartCoroutine(WinCoroutine());
        }
    }

    private IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second before showing the win UI
        UIManager.instance.ShowUI(UIManager.GameUI.Win);
    }

    public void ShowRMAlteration(int i)
    {
        LessRMCost.SetActive(true);
        LessRMCost.GetComponentInChildren<TextMeshProUGUI>().text = "In this turn cards will cost " + (-i) + " less";
    }

    public void HideRMAlteration()
    {
        LessRMCost.SetActive(false);
    }
}
