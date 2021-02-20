using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaking : MonoBehaviour
{
    public float speed = 0.1f;
    private int direction = 1;

    private void Start()
    {
        InvokeRepeating("ChangeDirection", 0, 1);
    }

    private void ChangeDirection()
    {
        direction *= -1;
    }

    private void Update()
    {
        transform.Rotate(0, 0, speed * direction);
    }
}
