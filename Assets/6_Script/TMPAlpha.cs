using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPAlpha : MonoBehaviour
{
    [SerializeField] float lerpTime = 0.5f;
    TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    public void FadeOut()
    {
        StartCoroutine(AlphaLerp(1f, 0f));
    }
    IEnumerator AlphaLerp(float start, float end)
    {
        float currentTime = 0f;
        float percent = 0f;
        while (percent < 1f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / lerpTime;
            Color color = text.color;
            color.a = Mathf.Lerp(start, end, percent);
            text.color = color;
            yield return null;
        }
    }
}
