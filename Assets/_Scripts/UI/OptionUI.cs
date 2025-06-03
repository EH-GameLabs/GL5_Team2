using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionUI : BaseUI
{
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
}
