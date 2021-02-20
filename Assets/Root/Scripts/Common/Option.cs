using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    public GameObject PassOverlay;
    public GameObject PassResult;
    public GameObject FailOverlay;
    public GameObject FailResult;

    [SerializeField] GameObject optionLeft;
    [SerializeField] GameObject optionRight;
    [SerializeField] bool isSwap = false;
    [SerializeField] bool isRandomSwap = true;
    [SerializeField] private float offsetBottom = 200;
    [NonSerialized] public Const.Common.WAVE_STATE state;
    bool isClicked = false;

    private void Start()
    {
        if (isSwap)
        {
            SwapOption();
        }
        else if (isRandomSwap)
        {
            var random = UnityEngine.Random.Range(-1, 1);
            if (random >= 0)
            {
                SwapOption();
            }
        }
        
        SetOffsetBottom();
    }

    private void SetOffsetBottom()
    {
        Vector3 position = optionLeft.transform.position;
        position.y += offsetBottom;
        optionLeft.transform.position = position;

        position = optionRight.transform.position;
        position.y += offsetBottom;
        optionRight.transform.position = position;
    }

    private void SwapOption()
    {
        var position = optionLeft.transform.position;
        optionLeft.transform.position = optionRight.transform.position;
        optionRight.transform.position = position;
    }

    public void OnClickOption(bool state)
    {
        if (!isClicked)
        {
            this.state = state ? Const.Common.WAVE_STATE.PASS : Const.Common.WAVE_STATE.FAIL;
            isClicked = true;

            ShowOverlay();

            if (state)
            {
                Gamemanager.Instance.CurrentLevel.OnPass();
            } else
            {
                Gamemanager.Instance.CurrentLevel.OnFail();
            }
        }
    }

    public void ShowOverlay()
    {
        GameObject overlay = state == Const.Common.WAVE_STATE.PASS ? PassOverlay : FailOverlay;
        overlay.SetActive(true);
    }

    public void ShowImg()
    {
        GameObject image = state == Const.Common.WAVE_STATE.PASS ? PassResult : FailResult;
        image.SetActive(true);
    }
}
