using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }

        if (collision.CompareTag("Movable Object"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }

        if (collision.CompareTag("Movable Object"))
        {
            collision.transform.SetParent(null);
        }
    }
}
