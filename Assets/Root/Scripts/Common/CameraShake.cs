using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private bool isShaking = false;
    [SerializeField] private float shakeAmount = 0.7f;
    [SerializeField] private float decreaseFactor = 1.0f;

    void Update()
    {
        if (isShaking)
        {
            Camera.main.transform.localPosition = Camera.main.transform.localPosition + Random.insideUnitSphere * shakeAmount;
        }
    }

    public void ChangeAmount(float amount)
    {
        shakeAmount = amount;
    }

    public void Shake() {
        isShaking = true;
    }

    public void StopShake() {
        isShaking = false;
    }
}