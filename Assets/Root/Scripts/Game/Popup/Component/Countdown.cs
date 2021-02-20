using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    float time = 5;

    private void Start()
    {
        InvokeRepeating("DecreaseTime", 1, 1);
    }

    public void DecreaseTime()
    {
        if (time >= 0)
        {
            GetComponent<TextMeshProUGUI>().text = time.ToString();
            time--;
        } else
        {
            CancelInvoke();
            Gamemanager.Instance.NextWave();
            Gamemanager.Instance.LoadLevel();
        }
    }
}
