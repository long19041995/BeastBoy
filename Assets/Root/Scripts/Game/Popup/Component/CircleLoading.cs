using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CircleLoading : MonoBehaviour
{
    Image fillImage;
    float time = 6;

    public void Start()
    {
        fillImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            fillImage.fillAmount = time / 5;
        }
    }
}
