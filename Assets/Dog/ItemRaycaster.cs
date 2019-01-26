using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRaycaster : MonoBehaviour
{
    [Header("The dog's camera's transform")]
    public Transform cameraTranform;
    [Header("How far can the dog reach things")]
    public float reachLenght = 1f;

    void Start()
    {

    }

    void Update()
    {
        ItemController item = GetItemInRange();
        if (item != null)
        {
            item.gameObject.GetComponent<ItemHighlighter>().HighlightNextNSeconds(Time.deltaTime * 2f);
            if (Input.GetMouseButtonDown(0))
            {
                item.SniffItem();
            }
        }
    }

    ItemController GetItemInRange()
    {
        // Ignore the "ignore raycast" layer
        int layerMask = 1 << 2;
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(cameraTranform.position, cameraTranform.TransformDirection(Vector3.forward), out hit, reachLenght, layerMask))
        {
            return hit.collider.gameObject.GetComponent<ItemController>();
        }
        else
        {
            return null;
        }
    }
}
