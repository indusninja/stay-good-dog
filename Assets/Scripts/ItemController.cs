using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public ItemSpawner[] itemConnections;

    public void SetItemSpawnerConnections(ItemSpawner[] spawners)
    {
        itemConnections = spawners;
    }
}
