using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POI : Highlightable
{
    private float disableInSeconds = 0f;

    void Update()
    {
        if (disableInSeconds <= 0)
        {
            gameObject.SetActive(false);
        }

        disableInSeconds -= Time.deltaTime;
    }

    override public void HighlightForNSeconds(float seconds)
    {
        Debug.LogWarning("HIGHLIGH IT !");
        disableInSeconds = seconds;
        gameObject.SetActive(true);
    }
}
