using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : BaseUI
{

    [SerializeField] private GameObject popupPanel;

    private void OnEnable()
    {
        popupPanel.SetActive(false);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void GoToHud()
    {
        UIManager.instance.ShowUI(UIManager.GameUI.HUD);
        Time.timeScale = 1f;
        SoundManager.Instance.ResumeMusic();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GoToOptions()
    {
        FindAnyObjectByType<OptionUI>(FindObjectsInactive.Include).backPanel = GetUIType();
        UIManager.instance.ShowUI(UIManager.GameUI.Option);
    }
}
