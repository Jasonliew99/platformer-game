using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class PlayerMovement : Movement
{
    protected override void HandleInput()
    {
        if (isDashing)
        {
            return;
        }

        _inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetButton("Jump"))
        {
            DoJump();
            _isJumping = true;
        }
        else
        {
            _isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && canDash)
        {
            StartCoroutine(Dash());

        }


    }
}
