using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIFadeInOut : MonoBehaviour
{
    public bool fadeInOnStart;
    public float fadeInTime;

    private void Start()
    {
        if (fadeInOnStart) FadeIn(fadeInTime);
    }

    public void FadeIn(float fadeDuration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInOut(true, fadeDuration));
    }
    public void FadeOut(float fadeDuration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInOut(false, fadeDuration));
    }

    IEnumerator FadeInOut(bool fadeIn, float fadeTime)
    {
        Image panelImage = GetComponent<Image>();
        Color panelColor = panelImage.color;

        float timer = 0;
        float currentState;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeTime);

            if (fadeIn) currentState = Mathf.Lerp(1f, 0f, t);
            else currentState = Mathf.Lerp(0f, 1f, t);

            panelColor.a = currentState;
            panelImage.color = panelColor;

            yield return null;
        }
    }
}
