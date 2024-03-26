using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaMovement : Movement
{
    protected bool FlipDirection = false;

    protected override void HandleInput()
    {
        if (FlipDirection)
        {
            Debug.Log(" goomba moving left");
            _inputDirection = Vector2.left;
        }
        else
        {
            Debug.Log("goomba moving right");
            _inputDirection = Vector2.right;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("boundary"))
            return;

        FlipDirection = !FlipDirection;
    }

}
