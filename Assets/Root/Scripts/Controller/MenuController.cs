using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : Singleton<MenuController>
{
    [SerializeField] GameObject home;
    [SerializeField] GameObject level;
    [SerializeField] GameObject settings;

    private async void Start()
    {
        await Util.Delay(2);

        AudioController.Instance.Play(Const.Common.AUDIOS.UI_BACKGROUND, true, 0.5f);
        AudioController.Instance.Stop(Const.Common.AUDIOS.IN_GAME_BACKGROUND);
    }

    public void ShowHome()
    {
        home.SetActive(true);
        level.SetActive(false);
    }

    public void ShowLevel()
    {
        level.SetActive(true);
        home.SetActive(false);
    }

    public void ShowSettings()
    {
        settings.SetActive(true);
    }

    public void HideSettings()
    {
        settings.SetActive(false);
    }
}
