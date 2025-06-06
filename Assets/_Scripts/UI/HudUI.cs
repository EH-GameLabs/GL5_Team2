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
    [SerializeField] private TextMeshProUGUI enemyHealthTextTMP;

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

    bool isLose = false;
    public void UpdatePlayerHealth(int health)
    {
        foreach (GameObject g in playerHealths)
        {
            g.SetActive(false);
        }
        playerHealths[health].SetActive(true);

        if (health <= 0)
        {
            if (!isLose)
                StartCoroutine(LoseCoroutine());
        }
    }

    private IEnumerator LoseCoroutine()
    {
        isLose = true;
        yield return new WaitForSeconds(1f);

        SoundManager.Instance.PauseMusic();

        UIManager.instance.ShowUI(UIManager.GameUI.Video);

        VideoUI videoUI = FindAnyObjectByType<VideoUI>(FindObjectsInactive.Include);
        videoUI.PlayVideo(VideoType.Lose);
    }

    bool isWin = false;
    public void UpdateEnemyHealth(float health, float maxHealth)
    {
        if (health < 0) health = 0;

        enemyHealthTextTMP.text = health.ToString();
        enemyHealthSlider.value = health / maxHealth;
        if (enemyHealthSlider.value <= 0)
        {
            if (!isWin)
                StartCoroutine(WinCoroutine());
        }
    }

    private IEnumerator WinCoroutine()
    {
        isWin = true;
        yield return new WaitForSeconds(1f); // Wait for 1 second before showing the win UI

        SoundManager.Instance.PauseMusic();

        UIManager.instance.ShowUI(UIManager.GameUI.Video);

        VideoUI videoUI = FindAnyObjectByType<VideoUI>(FindObjectsInactive.Include);
        videoUI.PlayVideo(VideoType.Win);
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
