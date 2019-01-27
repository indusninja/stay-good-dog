using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHighlighter : Highlightable
{

    private float disableInSeconds = 0;

    public void Update()
    {
        disableInSeconds = Mathf.Max(0, disableInSeconds - Time.deltaTime);
        if (disableInSeconds <= 0)
        {
            SetLayerRecursively(this.gameObject, 0);  // Default layer
       }
    }

    public override void HighlightForNSeconds(float seconds)
    {
        SetLayerRecursively(this.gameObject, 11);  // NoPost layer
        disableInSeconds = seconds;
    }

    void SetLayerRecursively(GameObject go, int newLayer) 
    {
        go.layer = newLayer;

        foreach (Transform t in go.transform)
        {
            SetLayerRecursively(t.gameObject, newLayer);
        }
    }

}