using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundInfinite : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float length = 21;
    [SerializeField] private bool moveOnWake = true;
    [SerializeField] private bool moveToLeft = true;

    private Vector2 startPosition;
    private bool isMove = false;
    private int direction = 1;
    private float startTime = 0;

    private void Start()
    {
        startPosition = transform.position;
        isMove = moveOnWake;
        direction = moveToLeft ? 1 : -1;
    }

    public void Move()
    {
        startTime = Time.time;
        isMove = true;
    }

    public void StopMove()
    {
        isMove = false;
    }

    private void Update()
    {
        if (isMove)
        {
            float position = Mathf.Repeat((Time.time - startTime) * speed, length);
            transform.position = startPosition - Vector2.right * position * direction;
        }
    }
}
