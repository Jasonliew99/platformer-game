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
    public float walkSpeed;//from yt
    public float jumpSpeed;//from yt

    public Cooldown CoyoteTime;
    public Cooldown BufferJump;

    public LayerMask GroundLayerMask;


    protected bool _isGrounded = false;
    protected bool _isJumping = false;
    protected bool _isRunning = false;
    protected bool _canJump = true;
    protected bool _bufferJump = true;
    protected bool _isFalling = false;

    private float moveInput;//from yt
    private bool isTouchingLeft;//from yt
    private bool isTouchingRight;//from yt
    private bool wallJumping;//from yt
    private float touchingLeftOrRight;//from yt

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
    }


    // Update is called once per frame
    void Update()
    {
        HandleInput();

        //from yt everything from here
        isTouchingLeft = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f), new Vector2(0.2f, 0.9f), 0f, GroundLayerMask);
        isTouchingRight = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f), new Vector2(0.2f, 0.9f), 0f, GroundLayerMask);

        if (isTouchingLeft)
        {
            touchingLeftOrRight = 1;
        }
        else if (isTouchingRight)
        {
            touchingLeftOrRight = -1;
        }

        if((!isTouchingLeft && !isTouchingRight) || IsGrounded)
        {
            rb.velocity = new Vector2(moveInput* walkSpeed, rb.velocity.y);
        }

        if(Input.GetKeyDown("space") && (isTouchingRight || isTouchingLeft) && !IsGrounded)
        {
            wallJumping = true;
            Invoke("SetJumpingToFalse", 0.08f);
        }

        if (wallJumping)
        {
            rb.velocity = new Vector2(walkSpeed * touchingLeftOrRight, jumpSpeed);
        }
        //to here
    }

    void FixedUpdate()
    {
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

    //here till
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f), new Vector2(1.3f, 0.2f));

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x - 0.7f, gameObject.transform.position.y + 0.3f), new Vector2(0.2f, 1.3f));
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x + 0.7f, gameObject.transform.position.y + 0.3f), new Vector2(0.2f, 1.3f));
    }

    void SetJumpingToFalse()
    {
        wallJumping = false;
    }
    //till here from yt
}
