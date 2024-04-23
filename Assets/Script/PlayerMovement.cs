using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using UnityEngine.SceneManagement;

public class PlayerMovement : Movement
{
    protected override void HandleInput()
    {
        if (isDashing)
        {
            return;
        }

        _inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Jump"))
        {
            DoJump();
            _isJumping = true;
        }
        //else
        //{
        //    _isJumping = false;
        //}


                if (Input.GetKeyDown(KeyCode.RightShift) && canDash)
        {
            StartCoroutine(Dash());

        }

        if (Input.GetButtonUp("Jump") )
        {
            _canJump = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
