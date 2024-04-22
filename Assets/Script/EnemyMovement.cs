using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovement : Movement
{
    public Transform Target;
    public GameObject Player;

    protected override void HandleInput()
    {
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (Target == null)
            return;

        Vector2 targetDirection = Target.position - transform.position;
        targetDirection = targetDirection.normalized;

        _inputDirection = targetDirection;


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Destroy(collision.gameObject); to destroy player
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        
    }
}

//inputDirection = new Vector2(1,0) makes it go right
//inputDirection = new Vector2(-1,0) makes it go left
