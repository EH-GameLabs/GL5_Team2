using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : BaseUI
{
    public void GoToHud()
    {
        SoundManager.Instance.PauseMusic();
        UIManager.instance.ShowUI(UIManager.GameUI.Video);
    }

    public void GoToOptions()
    {
        FindAnyObjectByType<OptionUI>(FindObjectsInactive.Include).backPanel = GetUIType();
        UIManager.instance.ShowUI(UIManager.GameUI.Option);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
