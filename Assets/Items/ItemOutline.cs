using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOutline : MonoBehaviour
{

    private Outline outline;
    private float disableInSeconds = 0;

    void Start()
    {
        outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;
    }

    public void Update()
    {
        disableInSeconds = Mathf.Max(0, disableInSeconds - Time.deltaTime);
        if (disableInSeconds <= 0)
        {
            outline.enabled = false;
        }
    }
    public void DrawOutlineNextSeconds(float s)
    {
        outline.enabled = true;
        disableInSeconds = s;
    }
}