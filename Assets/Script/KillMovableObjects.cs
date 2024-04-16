using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KillMovableObjects : MonoBehaviour
{
    public Transform respawnPoint; // Respawn point for the object

    private void OnTriggerEnter2D(Collider2D col)
    {

        Debug.Log("test");
        if (!col.CompareTag("Movable Object Killer"))
            return;

        transform.position = respawnPoint.position; //respawn point where?
    }
}
