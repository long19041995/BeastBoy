using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progress : Singleton<Progress>
{
    public List<GameObject> listProgress;
    public int index;
    public Sprite Normal;
    public Sprite Pass;
    public Sprite Fail;
    public Text From;
    public Text To;

    private void Start()
    {
        From.text = (DataController.Instance.IndexLevel + 1).ToString();
        To.text = (DataController.Instance.IndexLevel + 2).ToString();
        SetToDefault();
        SetToPass();
    }

    private void SetToDefault()
    {
        for (int i = 0; i < listProgress.Count; i++)
        {
            SetNormal(i);
        }
    }

    private void SetToPass()
    {
        for (int i = 0; i < DataController.Instance.IndexWave; i++)
        {
            SetPass(i);
        }
    }

    public void SetState()
    {
        int index = DataController.Instance.IndexWave;

        if (OptionController.Instance.GetState() == Const.Common.WAVE_STATE.PASS)
        {
            SetPass(index);
        }

        if (OptionController.Instance.GetState() == Const.Common.WAVE_STATE.FAIL)
        {
            SetFail(index);
        }
    }

    public void SetNormal(int index)
    {
        SetSprite(Normal, index);
    }

    public void SetPass(int index)
    {
        SetSprite(Pass, index);
    }

    public void SetFail(int index)
    {
        SetSprite(Fail, index);
    }

    private void SetSprite(Sprite sprite, int index)
    {
        listProgress[index].GetComponent<Image>().sprite = sprite;
    }
}
