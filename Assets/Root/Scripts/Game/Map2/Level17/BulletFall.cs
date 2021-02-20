using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFall : MonoBehaviour
{
    private Vector2 originPosition;

    private async void Start()
    {
        originPosition = transform.position;

        await Util.Delay(Random.Range(0f, 10f));
        GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Collider")
        {
            transform.position = originPosition;
        }
    }
}
