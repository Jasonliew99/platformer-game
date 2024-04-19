using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killEnemy : MonoBehaviour
{
    public Transform respawnPoint; // Respawn point for the object

    private void OnTriggerEnter2D(Collider2D col)
    {

        Debug.Log("Triggered enemy killer");
        if (!col.CompareTag("Enemy Killer"))
            return;

        transform.position = respawnPoint.position; //respawn point where?
    }
}
