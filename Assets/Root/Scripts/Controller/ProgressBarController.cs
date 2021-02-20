using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarController : Singleton<ProgressBarController>
{
    GameObject currentProgressBar;

    public void Init(GameObject progressBar)
    {
        Destroy();
        currentProgressBar = progressBar;
    }

    public void Destroy()
    {
        if (currentProgressBar != null)
        {
            Destroy(currentProgressBar);
        }
    }

    public void SetActive(bool state)
    {
        currentProgressBar.SetActive(state);
    }

    public void SetState()
    {
        currentProgressBar.GetComponent<Progress>().SetState();
    }
}
