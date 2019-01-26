﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ItemSpawner : MonoBehaviour
{
    public ItemController itemPrefab;
    public bool isGrave = false;
    public ItemSpawner[] itemConnections;

    public ItemController myItem;

    private bool isSelected = false;
    private bool propergateSelection = false;
    private Color connectionColor;
    private bool itemActivated = false;

    private bool illegalConnection = false;

    public void Start()
    {
        SetupItem();
    }

    /*
     * This sets up the instance of the 
     */
    void SetupItem()
    {
        if (itemPrefab)
        {
            GameObject item = Instantiate(itemPrefab.gameObject, transform.position, transform.rotation, transform) as GameObject;
            item.transform.parent = transform;
            myItem = item.GetComponent<ItemController>();

            // Pass over spawner connections to Item
            // Todo - Change to actual item instances instead of parent spawners of render items
            myItem.SetItemSpawnerConnections(itemConnections);

            // Deactivate the item instance after setting it up
            item.SetActive(false);
        }
    }

    public void RevealItem()
    {
        if (!itemActivated)
        {
            itemActivated = true;
            // Activate item
            myItem.gameObject.SetActive(true);
            itemActivated = true;
        }
    }

    // Returns list of spawners connections 
    public ItemSpawner[] GetConnections()
    {
        return itemConnections;
    }

    // Returns connected item instance (real rednered item)
    public ItemController GetMyConnectedItem()
    {
        return myItem;
    }

    private void Update()
    {
        EditorSelectionCheck();
        RenderDebugLines();
    }

    /*
     * Check if this item is selected in editor
     */
    void EditorSelectionCheck()
    {
        if (isSelected != Selection.Contains(gameObject))
        {
            isSelected = Selection.Contains(gameObject);
            PropergateSelection(isSelected, gameObject);
        }
    }

    public void PropergateSelection(bool propergate, GameObject originConnection)
    {
        illegalConnection = false;
        propergateSelection = propergate;

        // Try to prevent bi directional connections loops
        foreach (ItemSpawner connection in itemConnections)
        {
            if (connection && originConnection == connection.gameObject)
            {
                illegalConnection = true;
                Debug.LogError("Connections are illegally pointing to eachother");
                return;
            }
        }

        foreach (ItemSpawner connection in itemConnections)
        {
            if (connection != null)
            {
                connection.PropergateSelection(propergateSelection, originConnection);
            }
        }
    }

    /*
     * Render debug link lines
     */
    void RenderDebugLines()
    {
        if (propergateSelection)
        {
            for (var i = 0; i < itemConnections.Length; i++)
            {
                ItemSpawner connection = itemConnections[i];
                if (connection != null && connection.gameObject != gameObject)
                {
                    Color color = Color.blue;
                    if (illegalConnection)
                    {
                        color = Color.red;
                    }
                    Debug.DrawLine(transform.position, connection.transform.position, color);
                }
            }
        }
    }
}
