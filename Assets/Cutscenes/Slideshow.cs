using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slideshow : MonoBehaviour
{
    public Slide[] slides;

    private RawImage bottomImageRenderer;
    private RawImage topImageRenderer;

    public GameObject fadeToGame;

    void Start()
    {
        topImageRenderer = transform.GetChild(0).GetComponent<RawImage>();
        bottomImageRenderer = GetComponent<RawImage>();
        StartSlideshow();
    }

    public void StartSlideshow()
    {
        StartCoroutine(DoSlideshow());
    }

    IEnumerator DoSlideshow()
    {
        for (int i = 0; i < slides.Length; i++)
        {
            bottomImageRenderer.texture = slides[i].Image;
            for (int n = 0; n < 100; n++)
            {
                Color tempColor = topImageRenderer.color;
                tempColor.a -= 0.01f;
                topImageRenderer.color = tempColor;
                yield return new WaitForSeconds(0.02f);
            }
            topImageRenderer.texture = bottomImageRenderer.texture;
            topImageRenderer.color = Color.white;
            yield return new WaitForSeconds(slides[i].seconds);
            if(i == slides.Length - 1) {
                Debug.Log("Length " + i);
                if(fadeToGame != null)
                {
                    StartCoroutine(DisableAfter());
                    StartCoroutine(FadeToBlack());
                } 
            }
        }
    }

    IEnumerator DisableAfter()
    {
        yield return new WaitForSeconds(0.8f);
        GetComponent<Canvas>().enabled = false;
    }

    IEnumerator FadeToBlack()
    {
        fadeToGame.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        fadeToGame.SetActive(false);
    }
}



[System.Serializable]
public struct Slide 
{
    public Texture Image;
    public float seconds;
}
