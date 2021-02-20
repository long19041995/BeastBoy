using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleLoop : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 1.1f;

    private bool isScale = false;
    private bool isActive = true;

    private void Start()
    {
        ChangeScaleUp();
    }

    private async void ChangeScaleUp()
    {
        if (!isActive) return;

        gameObject.transform.DOScale(maxScale, duration);

        await Util.Delay(duration);
        ChangeScaleDown();
    }

    private async void ChangeScaleDown()
    {
        if (!isActive) return;

        gameObject.transform.DOScale(minScale, duration);

        await Util.Delay(duration);
        ChangeScaleUp();
    }

    private void OnEnable()
    {
        isActive = true;
    }

    private void OnDisable()
    {
        isActive = false;
    }
}
