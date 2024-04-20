using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GridBrushBase;

//public class WallJump : MonoBehaviour
//{
//    private bool isWallSliding;
//    private float wallSlidingSpeed = 2f;

//    private bool isWalled;
//    private bool isWallJumping;
//    private float wallJumpingDirection;
//    private float walljumpingTime = 0.2f;
//    private float wallJumpingCounter;
//    private float wallJumpingDuration = 0.4f;
//    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

//    [SerializeField] private Transform wallCheck;

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.tag == "Wall")
//        {
//            isWalled = true;
//        }
//    }

//    private void WallSlide()
//    {
//        if (isWalled() && !_isGrounded() && Mathf.Abs(_inputDirection.x) != 0f)
//        {
//            isWallSliding = true;
//            _rigidbody2D.velocity = Vector2.(_rigidbody2D.velocity.x, Mathf.Clamp(_rigidbody2D.velocity.y, -wallSlidingSpeed, float.MaxValue));
//        }

//        else
//        {
//            isWallSliding = false;
//        }
//    }

//    private void WallJump()
//    {
//        if (isWallSliding)
//        {
//            isWallJumping = false;
//            wallJumpingDirection = -transform.localScale.x;
//            wallJumpingCounter = walljumpingTime;

//            CancelInvoke(nameof(StopWallJumping));
//        }

//        else
//        {
//            wallJumpingCounter -= Time.deltaTime;
//        }

//        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
//        {
//            isWallJumping = true;
//            _rigidbody2D.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);

//            if (_inputDirection.x > 0)
//            {
//                FlipAnim = false;
//            }

//            else if (_inputDirection.x < 0)
//            {
//                FlipAnim = true;

//            }

//            Invoke(nameof(StopWallJumping), wallJumpingDuration);
//        }
//    }

//    private void StopWallJumping()
//    {
//        isWallJumping = false;
//    }
//}
