using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dark : MonoBehaviour
{
    SpriteRenderer render;
    float fadeTime = 0.05f;
    private float maxOpacity = 0;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    public void SetFadeTime(float fadeTime)
    {
        this.fadeTime = fadeTime;
    }

    public void FadeIn(float maxOpacity = 0)
    {
        this.maxOpacity = maxOpacity;
        StartCoroutine("StartFade", fadeTime);
    }

    public void FadeOut()
    {
        StartCoroutine("StartFade", -fadeTime);
    }

    private IEnumerator StartFade(float speedTime)
    {
        float opacity = speedTime > 0 ? 0 : maxOpacity;
        while (opacity >= 0 && opacity <= maxOpacity)
        {
            ChangeOpacity(opacity);
            yield return new WaitForSeconds(fadeTime);
            opacity += speedTime;
        }
    }

    private void ChangeOpacity(float opacity)
    {
        Color c = render.color;
        c.a = opacity;
        render.color = c;
    }
}
