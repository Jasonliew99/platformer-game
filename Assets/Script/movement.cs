using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float Acceleration = 10f;
    public float JumpForce = 50f;


    //Ground check
    public Transform GroundCheck;
    public float GroundCheckRadius = 1f;
    public float MaxSlopeAngle = 45f;


    public LayerMask GroundLayerMask;


    private bool _isGrounded = false;
    private bool _isJumping = false;

    protected Vector2 _inputDirection;


    protected Rigidbody2D _rigidbody2D;
    protected Collider2D _collider2D;


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

    void HandleInput()
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

    void DoJump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, JumpForce);
    }

    void HandleMovement()
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
    }

    void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundLayerMask);

        if (_isGrounded)
        {
                
            if (_isJumping)
            {
                DoJump();
            }
        }
    }
}
