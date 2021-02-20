using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Flickering : MonoBehaviour
{
    public float minOpacity = 0;
    public float maxOpacity = 1;
    public float speed = 0.5f;
    public float speedTime = 0.1f;
    public bool isRandomStartTime = true;
    public bool startWhenAwake = false;

    private Renderer render;
    private Coroutine coroutine;

    private void Start()
    {
        render = GetComponent<Renderer>();
    }

    public void OnEnable()
    {
        if (startWhenAwake)
        {
            StartFlicker();
        }
    }

    public async void StartFlicker()
    {
        float startTime = 0;
        if (isRandomStartTime)
        {
            startTime = Random.Range(0f, 2f);
        }

        int delay = (int)(startTime * 1000);
        await Task.Delay(delay);

        coroutine = StartCoroutine("Flicker");
    }

    public void StopFlicker()
    {
        StopCoroutine(coroutine);
    }

    IEnumerator Flicker()
    {
        float opacity = minOpacity;
        while (true)
        {
            opacity += speed;
            if (opacity < minOpacity || opacity > maxOpacity)
            {
                speed *= -1;
            }

            ChangeOpacity(opacity);

            yield return new WaitForSeconds(speedTime);
        }
    }

    public virtual void ChangeOpacity(float opacity)
    {
        if (render is SpriteRenderer)
        {
            SpriteRenderer r = (SpriteRenderer)render;
            Color c = r.color;
            c.a = opacity;
            r.color = c;
        }

        if (render is LineRenderer)
        {
            LineRenderer r = (LineRenderer)render;

            Color startColor = r.startColor;
            startColor.a = opacity;
            r.startColor = startColor;

            Color endColor = r.endColor;
            endColor.a = opacity;
            r.endColor = endColor;
        }
    }
}
