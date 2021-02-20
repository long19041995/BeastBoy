using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : Singleton<OptionController>
{
    GameObject currentOption;

    public void Init(GameObject option)
    {
        Destroy(currentOption);
        currentOption = option;
        if (DataController.Instance.CurrentWaveData.hideOptionOnLoad)
        {
            currentOption.SetActive(false);
        }
    }

    public void Destroy()
    {
        if (currentOption != null)
        {
            Destroy(currentOption);
        }
    }

    public void SetActive(bool state)
    {
        currentOption.SetActive(state);
    }

    public void ShowOverlay()
    {
        currentOption.GetComponent<Option>().ShowOverlay();
    }

    public void ShowImg()
    {
        currentOption.GetComponent<Option>().ShowImg();
    }

    public Const.Common.WAVE_STATE GetState()
    {
        return currentOption.GetComponent<Option>().state;
    }
}
