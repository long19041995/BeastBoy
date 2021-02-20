using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coop : MonoBehaviour
{
    [SerializeField] private GameObject laser1;
    [SerializeField] private GameObject laser2;
    private bool isLaser1 = true;

    private void Start()
    {
        InvokeRepeating("LoopLaser", 0, 0.1f);
    }

    public void StopInvoke()
    {
        CancelInvoke();
        laser1.SetActive(false);
        laser2.SetActive(false);
    }

    private void LoopLaser()
    {
        isLaser1 = !isLaser1;

        if (isLaser1)
        {
            laser1.SetActive(true);
            laser2.SetActive(false);
        }
        else
        {
            laser2.SetActive(true);
            laser1.SetActive(false);
        }
    }
}
