using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogKillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("OnTriggerEnter KILL ZONE");
        if (collider.transform.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<DogController> ().Kill();
        }
    }
}
