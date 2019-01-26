using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public bool isGrave = false;
    public ItemSpawner[] itemConnections;

    private bool isSelected = false;
    private bool propergateSelection = false;
    private Color connectionColor;

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
            PropergateSelection(isSelected);
        }
    }

    public void PropergateSelection(bool propergate)
    {
        connectionColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        propergateSelection = propergate;

        foreach (ItemSpawner connection in itemConnections)
        {
            if (connection != null)
            {
                connection.PropergateSelection(propergateSelection);
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
                if (connection != null)
                {
                    Debug.DrawLine(transform.position, connection.transform.position, connectionColor);
                }
            }
        }
    }
}
