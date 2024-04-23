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

    //private RaycastHit2D _slopeHit;
    //private RaycastHit2D _slopeHitFront;
    //private RaycastHit2D _slopeHitBack;
    //public bool IsOnSlope = false;

    //public PhysicsMaterial2D Default;
    //public PhysicsMaterial2D FullFriction;

    //private float _slopeSideAngle = 0f;
    //private float _slopeDownAngle = 0f;
    //private float _lastSlopAngle = 0f;
    //private bool _canWalkOnSlope = false;

    //private Vector2 _slopeNormalPerpendicular = Vector2.zero;

    public Cooldown CoyoteTime;
    public Cooldown BufferJump;

    public LayerMask GroundLayerMask;

    AudioSource jumpsound;

    protected bool _isGrounded = false;
    protected bool _isJumping = false;
    protected bool _isRunning = false;
    protected bool _canJump = true;
    protected bool _bufferJump = true;
    protected bool _isFalling = false;

    protected Vector2 _inputDirection;

    protected Rigidbody2D _rigidbody2D;
    protected Collider2D _collider2D;

    //private float m_MovementSmoothing = .05f;
    //private Vector3 m_Velocity = Vector3.zero;

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

        if (_isGrounded)
        {
            _canJump = true;
        }
        //CheckSlope();

        HandleMovement();

        Flip();
    }

    protected virtual void HandleInput()
    {

    }

    protected virtual void DoJump()
    {
        //we need to do cooldown check
        //TryBufferJump();

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
        if (_rigidbody2D == null)
            return;

        Vector3 targetVelocity = Vector3.zero;

        //if (IsGrounded && !IsOnSlope && !IsJumping)
        //{
        //    targetVelocity = new Vector2(_inputDirection.x * (Acceleration), 0f);
        //    _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        //}
        //else if (IsGrounded && IsOnSlope && _canWalkOnSlope && !IsJumping)
        //{
        //    targetVelocity = new Vector2(-_inputDirection.x * (Acceleration) * _slopeNormalPerpendicular.x,
        //        -_inputDirection.x * (Acceleration) * _slopeNormalPerpendicular.y);
        //    _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        //}
        //else if (!IsGrounded)
        //{
        //    targetVelocity = new Vector2(_inputDirection.x * (Acceleration), _rigidbody2D.velocity.y);
        //    _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        //}


        //if (targetVelocity.x == 0)
        //{
        //    _isRunning = false;
        //}
        //else
        //{
        //    _isRunning = true;
        //}

        //Vector3 targetVelocity = Vector3.zero;

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
                Debug.Log("Buffer jump");
                DoJump();
            }


        }

        if (!_isGrounded && !_isJumping && CoyoteTime.CurrentProgress == Cooldown.Progress.Ready)
            CoyoteTime.StartCooldown();
    }

    //protected void CheckSlope()
    //{
    //    CheckSlopeHorizontal();
    //    CheckSlopeVertical();
    //}

    //protected void CheckSlopeHorizontal()
    //{
    //    _slopeHitFront = Physics2D.Raycast(GroundCheck.position, Vector2.right, 1f, GroundLayerMask);
    //    _slopeHitBack = Physics2D.Raycast(GroundCheck.position, Vector2.left, 1f, GroundLayerMask);

    //    if (_slopeHitFront)
    //    {
    //        IsOnSlope = true;
    //        _slopeSideAngle = Vector2.Angle(Vector2.up, _slopeHitFront.normal);
    //    }
    //    else if (_slopeHitBack)
    //    {
    //        IsOnSlope = true;
    //        _slopeSideAngle = Vector2.Angle(Vector2.up, _slopeHitBack.normal);
    //    }
    //    else
    //    {
    //        _slopeSideAngle = 0f;
    //        IsOnSlope = false;
    //    }

    //}

    //protected void CheckSlopeVertical()
    //{
    //    _slopeHit = Physics2D.Raycast(GroundCheck.position, Vector2.down, 1f, GroundLayerMask);

    //    if (_slopeHit)
    //    {
    //        _slopeNormalPerpendicular = Vector2.Perpendicular(_slopeHit.normal).normalized;

    //        _slopeDownAngle = Vector2.Angle(_slopeHit.normal, Vector2.up);

    //        if (_slopeDownAngle != _lastSlopAngle)
    //        {
    //            IsOnSlope = true;
    //        }

    //        _lastSlopAngle = _slopeDownAngle;

    //        Debug.DrawRay(_slopeHit.point, _slopeNormalPerpendicular, Color.blue);
    //        Debug.DrawRay(_slopeHit.point, _slopeHit.normal, Color.green);
    //    }

    //    if (_slopeDownAngle > MaxSlopeAngle || _slopeSideAngle > MaxSlopeAngle)
    //    {
    //        _canWalkOnSlope = false;
    //    }
    //    else
    //    {
    //        _canWalkOnSlope = true;
    //    }

    //    if (IsOnSlope && _canWalkOnSlope && _inputDirection.x == 0)
    //    {
    //        _rigidbody2D.sharedMaterial = FullFriction;
    //    }
    //    else if ( _inputDirection.x == 0)
    //    {
    //        _rigidbody2D.sharedMaterial = FullFriction;
    //    }
    //    else
    //    {
    //        _rigidbody2D.sharedMaterial = Default;
    //    }
    //}

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

