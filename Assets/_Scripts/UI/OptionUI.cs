using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionUI : BaseUI
{
    [Header("Glossary Pages")]
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject glossaryPanelPage1;
    [SerializeField] private GameObject glossaryPanelPage2;

    [Header("Back Panel")]
    public UIManager.GameUI backPanel;

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToHud()
    {
        UIManager.instance.ShowUI(UIManager.GameUI.HUD);
    }

    public void SetMusicVolume(Slider input)
    {
        SoundManager.Instance.SetMusicVolume(input.value);
    }

    public void SetSFXVolume(Slider input)
    {
        SoundManager.Instance.SetSFXVolume(input.value);
    }

    public void GoBack()
    {
        UIManager.instance.ShowUI(backPanel);
        SetPanelActive(audioPanel);
    }

    public void SetPanelActive(GameObject panel)
    {
        audioPanel.SetActive(audioPanel == panel);
        glossaryPanelPage1.SetActive(glossaryPanelPage1 == panel);
        glossaryPanelPage2.SetActive(glossaryPanelPage2 == panel);
    }
}
