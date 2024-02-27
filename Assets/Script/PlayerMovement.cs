using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : movement
{
    protected override void HandleInput()
    {
        _inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetButton("Jump"))
        {
            _isJumping = true;
        }
        else
        {
            _isJumping = false;
        }
    }
}
