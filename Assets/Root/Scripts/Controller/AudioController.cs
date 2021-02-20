using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : Singleton<AudioController>
{
    [SerializeField] private List<AudioClip> audioClips;

    private List<AudioSource> audioSources = new List<AudioSource>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < audioClips.Count; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClips[i];

            audioSources.Add(audioSource);
        }

        CheckMute();
    }

    public void Play(Const.Common.AUDIOS index, bool loop = false, float volumn = 1)
    {
        AudioSource audioSource = audioSources[(int)index];
        audioSource.loop = loop;
        audioSource.volume = volumn;
        audioSource.Play();
    }

    public void Stop(Const.Common.AUDIOS index)
    {
        AudioSource audioSource = audioSources[(int)index];
        audioSource.Stop();
    }

    public void Pause(Const.Common.AUDIOS index)
    {
        AudioSource audioSource = audioSources[(int)index];
        audioSource.Pause();
    }

    public void CheckMute()
    {
        CheckMuteSound();
        CheckMuteSoundAll();
    }

    private void CheckMuteSoundAll()
    {
        bool isMute = GetIsMuteSoundAll();
        SetMuteSoundAll(isMute);
    }

    private void CheckMuteSound()
    {
        bool isMute = GetIsMuteSound();
        SetMuteSound(isMute);
    }

    public void SetMuteSoundAll(bool isMute)
    {
        if (DataController.Instance.IsMuteSound)
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (audioSources[i].clip.name == "TouchUI1")
                {
                    audioSources[i].mute = isMute;
                }
            }
            return;
        }

        for (int i = 0; i < audioSources.Count; i++)
        {
            audioSources[i].mute = isMute;
        }
    }

    public void SetMuteSound(bool isMute)
    {
        if (DataController.Instance.IsMuteSoundAll)
        {
            return;
        }

        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].clip.name == "TouchUI1")
            {
                continue;
            }
            audioSources[i].mute = isMute;
        }
    }

    public bool ToggleSoundAll()
    {
        bool isMute = GetIsMuteSoundAll();
        SetIsMuteSoundAll(!isMute);

        isMute = GetIsMuteSoundAll();
        SetMuteSoundAll(isMute);
        return isMute;
    }

    public bool ToggleSound()
    {
        bool isMute = GetIsMuteSound();
        SetIsMuteSound(!isMute);

        isMute = GetIsMuteSound();
        SetMuteSound(isMute);
        return isMute;
    }

    private bool GetIsMuteSoundAll()
    {
        return DataController.Instance.IsMuteSoundAll;
    }

    private bool GetIsMuteSound()
    {
        return DataController.Instance.IsMuteSound;
    }

    private void SetIsMuteSoundAll(bool isMute)
    {
        DataController.Instance.IsMuteSoundAll = isMute;
    }

    private void SetIsMuteSound(bool isMute)
    {
        DataController.Instance.IsMuteSound = isMute;
    }
}
