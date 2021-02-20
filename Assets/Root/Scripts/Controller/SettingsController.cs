using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : Singleton<SettingsController>
{
    [SerializeField] private GameObject unMuteSoundAll;
    [SerializeField] private GameObject muteSoundAll;

    [SerializeField] private GameObject muteSound;

    private void Start()
    {
        SetMuteSound(DataController.Instance.IsMuteSound);
        SetMuteSoundAll(DataController.Instance.IsMuteSoundAll);
    }

    public void TapToSoundAll()
    {
        bool isMute = AudioController.Instance.ToggleSoundAll();
        SetMuteSoundAll(isMute);
    }

    public void TapToSound()
    {
        bool isMute = AudioController.Instance.ToggleSound();
        SetMuteSound(isMute);
    }

    private void SetMuteSoundAll(bool isMute)
    {
        muteSoundAll.SetActive(isMute);
        unMuteSoundAll.SetActive(!isMute);
    }

    private void SetMuteSound(bool isMute)
    {
        muteSound.SetActive(isMute);
    }
}
