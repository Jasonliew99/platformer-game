using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float Acceleration = 10f;
    public float JumpForce = 30f;

    [Header("Dashing Settings")]
    public bool canDash = true;
    public bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown;


    //Ground check
    [Header("Ground Check Settings")]
    public Transform GroundCheck;
    public float GroundCheckRadius = 1f;
    public float MaxSlopeAngle = 45f;

    public Cooldown CoyoteTime;
    public Cooldown BufferJump;

    public LayerMask GroundLayerMask;
    public LayerMask WallLayerMask;

    AudioSource jumpsound;
    AudioSource runsound;

    protected bool _isGrounded = false;
    protected bool _isJumping = false;
    protected bool _isRunning = false;
    protected bool _canJump = true;
    protected bool _bufferJump = true;
    protected bool _isFalling = false;

    protected Vector2 _inputDirection;

    protected Rigidbody2D _rigidbody2D;
    protected Collider2D _collider2D;

    public bool IsJumping
    {
        get { return _isJumping; }
    }

    public bool IsGrounded
    {
        get { return _isGrounded; }
    }

    public bool IsRunning
    {
        get { return _isRunning; }

    }

    public bool IsFalling
    {
        get { return _isFalling; }
    }

    public bool FlipAnim = false;

    // Start is called before the first frame update
    void Start()
    {
        //cache our components for later use
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        jumpsound = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        CheckGround();

        HandleMovement();

        Flip();
    }

    protected virtual void HandleInput()
    {

    }

    protected virtual void DoJump()
    {
        //we need to do cooldown check
        TryBufferJump();

        if (!_canJump)
            return;

        if (CoyoteTime.CurrentProgress == Cooldown.Progress.Finished)//don't need for walljump
            return;

        if (_canJump )
            jumpsound.Play();


        _canJump = false;
        _isJumping = true;

        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, JumpForce);

        CoyoteTime.StopCooldown();//don't need for walljump   

    }

    protected void TryBufferJump()
    {
        BufferJump.StartCooldown();
    }

    protected virtual void HandleMovement()
    {
        Vector3 targetVelocity = Vector3.zero;

        if (_isGrounded && !_isJumping)
        {
            targetVelocity = new Vector2(_inputDirection.x * (Acceleration), 0f);
        }
        else //in the air
        {
            targetVelocity = new Vector2(_inputDirection.x * (Acceleration), _rigidbody2D.velocity.y);
        }

        _rigidbody2D.velocity = targetVelocity;

        if (targetVelocity.x == 0)
        {
            _isRunning = false;

            //if (runsound.isPlaying)
            //    runsound.Stop();
        }
        else
        {
            _isRunning = true;
                
            //if (!runsound.isPlaying)
            //    runsound.Play();
            
        }
    }

    void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundLayerMask);

        if (_rigidbody2D.velocity.y <= 0)
        {
            _isJumping = false;
            //CoyoteTime.StopCooldown();
            _isFalling = true;
        }

        //else
        //{
        //    _isRunning = true;
        //}

        if (_isGrounded && !IsJumping)
        {
            _canJump = true;
            _isFalling = false;

            if (CoyoteTime.CurrentProgress != Cooldown.Progress.Ready)
                CoyoteTime.StopCooldown();

            if (BufferJump.CurrentProgress is Cooldown.Progress.Started or Cooldown.Progress.InProgress)
            {
                DoJump();
            }


        }

        if (!_isGrounded && !_isJumping && CoyoteTime.CurrentProgress == Cooldown.Progress.Ready)
            CoyoteTime.StartCooldown();
    }

    protected virtual void Flip()
    {
        if (_inputDirection.x == 0)
            return;

        if (_inputDirection.x > 0)
        {
            FlipAnim = false;
        }

        else if (_inputDirection.x < 0)
        {
            FlipAnim = true;

        }
    }

    protected virtual IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = _rigidbody2D.gravityScale;
        float dashDirection = _inputDirection.x > 0 ? 1f : -1f;
        _rigidbody2D.velocity = new Vector2(dashDirection * dashingPower, 0f); // Assign to velocity property
        yield return new WaitForSeconds(dashingTime);
        _rigidbody2D.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

}

