using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    public RawImage blackImage;
    public RawImage whiteImage;

    public bool BlackFadeIn = false;
    public bool BlackFadeOut = false;
    public bool WhiteFadeIn = false;
    public bool WhiteFadeOut = false;



    void FixedUpdate()
    {
        if(BlackFadeIn)
        {
            Color color = blackImage.color;
            Color newColor = new Color(color.r, color.g, color.b, 1.0f);
            blackImage.color = newColor;
            StartCoroutine(FadeTo(blackImage, 0.0f, 1.0f));
        }

        if (WhiteFadeIn)
        {
            StartCoroutine(FadeTo(whiteImage, 0.0f, 1.0f));
        }

        if (BlackFadeOut)
        {
            StartCoroutine(FadeTo(blackImage, 1.0f, 0.0f));
        }

        if (WhiteFadeOut)
        {
            StartCoroutine(FadeTo(whiteImage, 255.0f, 0.0f));
        }
    }

    IEnumerator FadeTo(RawImage image, float aValue, float aTime)
    {
        float alpha = image.color.a;
        Color color = image.color;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(color.r, color.g, color.b, Mathf.Lerp(alpha, aValue, t));
            image.color = newColor;
            yield return null;
        }
    }
}
