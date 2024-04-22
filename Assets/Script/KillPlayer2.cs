using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer2 : MonoBehaviour
{
    public GameObject Player;


    private void OnTriggerEnter2D(Collider2D collision) //use onTrigger if ticked on trigger box collider
    {
        if (collision.gameObject.tag == "Player")
        {
            //Destroy(collision.gameObject); to destroy player
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
