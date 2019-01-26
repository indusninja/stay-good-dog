using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOutline : MonoBehaviour
{

    private Outline outline;
    private int disableInFrames = 0;    // In how many frames to disable the outline

    void Start()
    {
        outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;
    }

    public void Update()
    {
        disableInFrames = Mathf.Max(0, disableInFrames - 1);
        if (disableInFrames == 0)
        {
            outline.enabled = false;
        }
    }
    public void DrawOutlineNextFrame()
    {
        outline.enabled = true;
        disableInFrames = 2;
    }
}