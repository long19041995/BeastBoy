using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCounter : MonoBehaviour
{
    [SerializeField] private float times = 100;

    private Text text;
    private float from;
    private float to;
    private float countPerTimes = 0;
    private float secondsPerTimes = 0;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    public void Init(float from, float to, float duration)
    {
        this.from = from;
        this.to = to;
        countPerTimes = (to - from) / times;
        secondsPerTimes = duration / times;

        StartCoroutine("Count");
    }

    private IEnumerator Count()
    {
        do
        {
            from += countPerTimes;
            text.text = ((int)from).ToString();
            yield return new WaitForSeconds(secondsPerTimes);
        } while (from != to);
    }
}
