using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{

    public ItemController[] connectedItems;

    public void SniffItem()
    {
        Debug.Log("Sniffed " + this.gameObject.name);
        foreach(ItemController item in connectedItems)
        {
            item.gameObject.GetComponent<ItemOutline>().DrawOutlineNextSeconds(6f);
        }
    }

    public ItemSpawner[] itemConnections;

    public void SetItemSpawnerConnections(ItemSpawner[] spawners)
    {
        itemConnections = spawners;
    }
}
