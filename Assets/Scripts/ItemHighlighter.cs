﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHighlighter : MonoBehaviour
{

    private float disableInSeconds = 0;

    public void Update()
    {
        disableInSeconds = Mathf.Max(0, disableInSeconds - Time.deltaTime);
        if (disableInSeconds <= 0)
        {
            gameObject.layer = 0; // Default layer
        }
    }
    public void HighlightNextNSeconds(float s)
    {
        gameObject.layer = 11;  // NoPost layer
        disableInSeconds = s;
    }
}