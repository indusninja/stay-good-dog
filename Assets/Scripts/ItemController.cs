using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if (UNITY_EDITOR)
using UnityEditor;
#endif
[ExecuteInEditMode]
public class ItemController : MonoBehaviour
{
    public ItemController[] connectedItems;

    public bool isGrave = false;
    public bool isCorrectGrave = false;
    public float highlightConnectionsForNSeconds = 10f;

    public DogController dogController;

    private bool isSelected = false;
    private bool propergateSelection = false;
    private Color connectionColor;
    private bool itemActivated = false;
    private bool illegalConnection = false;

    private SoundManager SoundManagerInstance = null;

    void Awake()
    {
        Highlightable highlighter = gameObject.GetComponent<Highlightable>();

        // get reference to the sound manager
        DogController[] foundDogControllers = FindObjectsOfType<DogController>();
        if (foundDogControllers.Length > 0)
        {
            SoundManagerInstance = foundDogControllers[0].GetComponentInChildren<SoundManager>();
        }
    }

    public void SniffItem()
    {
        if (isGrave)
        {
            if (isCorrectGrave)
            {
                Debug.Log("What a good boy, you found the grave");
                dogController.Win();
            }
            else
            {
                Debug.Log("Just a crap dog at the wrong grave");
                dogController.Kill();
            }
        }
        else
        {
            Debug.Log("Sniffed " + this.gameObject.name);
            // play sniff sound
            SoundManagerInstance.Sniff();
            // signal highlithing for the item
            foreach (ItemController item in connectedItems)
            {
                item.gameObject.GetComponent<Highlightable>().HighlightForNSeconds(highlightConnectionsForNSeconds);
            }
        }
    }

    // Returns list of spawners connections 
    public ItemController[] GetConnections()
    {
        return connectedItems;
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
#if (UNITY_EDITOR)
        if (isSelected != Selection.Contains(gameObject))
        {
            isSelected = Selection.Contains(gameObject);
            PropergateSelection(isSelected, gameObject);
        }
#endif
    }

    public void PropergateSelection(bool propergate, GameObject originConnection)
    {
        illegalConnection = false;
        propergateSelection = propergate;

        // Try to prevent bi directional connections loops
        foreach (ItemController connection in connectedItems)
        {
            if (connection && originConnection == connection.gameObject)
            {
                illegalConnection = true;
                Debug.LogError("Connections are illegally pointing to eachother");
                return;
            }
        }

        foreach (ItemController connection in connectedItems)
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
        if (connectedItems == null)
        {
            return;
        }
        if (propergateSelection)
        {
            for (var i = 0; i < connectedItems.Length; i++)
            {
                ItemController connection = connectedItems[i];
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
