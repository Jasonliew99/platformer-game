using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Acceleration = 10f;
    public float JumpForce = 50f;


    //Ground check
    public Transform GroundCheck;
    public float GroundCheckRadius = 1f;
    public float MaxSlopeAngle = 45f;

    public Cooldown CoyoteTime;
    public Cooldown BufferJump;

    public LayerMask GroundLayerMask;


    protected bool _isGrounded = false;
    protected bool _isJumping = false;
    protected bool _isRunning = false;
    protected bool _canJump = true;
    protected bool _bufferJump = true;

    protected Vector2 _inputDirection;


    protected Rigidbody2D _rigidbody2D;
    protected Collider2D _collider2D;

    public bool IsJumping
    {
        get { return _isJumping; }
    }

    public bool IsGrounded
    {
        get { return _isGrounded;  }
    }

    public bool IsRunning
    {
        get { return _isRunning; }
    }



    // Start is called before the first frame update
    void Start()
    {
        //cache our components for later use
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        CheckGround();

        HandleMovement();
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

        if (CoyoteTime.CurrentProgress == Cooldown.Progress.Finished)
            return;


        _canJump = false;
        _isJumping = true;

        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, JumpForce);

        CoyoteTime.StopCooldown();

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
        }
        else
        {
            _isRunning = true;
        }
    }

    void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundLayerMask);

        if(_rigidbody2D.velocity.y <= 0)
        {
            _isJumping = false;
        }

        //else
        //{
        //    _isRunning = true;
        //}

        if (_isGrounded && !IsJumping)
        {
            _canJump = true;

            if (CoyoteTime.CurrentProgress != Cooldown.Progress.Ready)
                CoyoteTime.StopCooldown();
                
            if(BufferJump.CurrentProgress is Cooldown.Progress.Started or Cooldown.Progress.InProgress)
            {
                DoJump();
            }


        }

        if(!_isGrounded && !_isJumping && CoyoteTime.CurrentProgress == Cooldown.Progress.Ready)
            CoyoteTime.StartCooldown();
    }
}
