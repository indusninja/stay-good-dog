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
            gameObject.layer = 0;
        }
    }
    public void DrawOutlineNextSeconds(float s)
    {
        gameObject.layer = 11;
        disableInSeconds = s;
    }
}