using System;
using Unity.VisualScripting;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private Transform orientation;

    [Header("Speed Controls")]
    [SerializeField] private float acceleration;
    [SerializeField] public float maxVelocity;

    [Header("Drag controls")]
    [SerializeField] private float groundDrag;
    [SerializeField] private LayerMask groundLayer;

    [Header("Ground")]
    public bool grounded;
    RaycastHit groundData;

    [Header("Jump controls")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float airControl;
    [SerializeField] private float airMaxVelocity;

    [Header("Dash controls")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldownTime;
    private float dashTimer = 0;
    private float dashCooldownTimer = 0;

    Rigidbody rb;

    Vector3 direction;

    enum MoveState
    {
        walking,
        jumping,
        dashing
    }

    MoveState mState = MoveState.walking;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        Timers();
        StateMachine();

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        direction = (orientation.forward * vertical + orientation.right * horizontal).normalized;

        GroundCast();
        Drag();
        LimitVelocity(direction);

        if(Input.GetKeyDown("space") && grounded)
            Jump();

        if(Input.GetKeyDown("left shift"))
            Dash(horizontal, vertical);
    }

    void FixedUpdate()
    {
        if(mState != MoveState.dashing)
            MovePlayer(direction);
    }

    private void StateMachine()
    {
        switch(mState)
        {
            case MoveState.walking:
                break;

            case MoveState.jumping:
            {
                if(rb.linearVelocity.y <= 0)
                {
                    mState = MoveState.walking;
                }
                break;
            }

            case MoveState.dashing:
            {
                if(dashTimer <= 0)
                {
                    mState = MoveState.walking;
                    rb.useGravity = true;
                }
                break;
            }
        }
    }

    private void GroundCast()
    {
        grounded = Physics.Raycast(orientation.position, Vector3.down, out groundData, 1.3f, groundLayer) && mState != MoveState.jumping;

        if(mState != MoveState.dashing)
            rb.useGravity = !grounded && Vector3.Angle(Vector3.up, groundData.normal) <= 45;
    }

    private void Drag()
    {
        if(grounded)
        {
            if(mState == MoveState.walking)
                rb.linearDamping = groundDrag;
            else
                rb.linearDamping = 0;
        }
        else
            rb.linearDamping = 0;
    }

    private float SlopeMultiplier(Vector3 direction)
    {
        return 1 + Mathf.Cos(Vector3.Angle(direction, groundData.normal) * Mathf.Deg2Rad) * .5f;
    }

    private void LimitVelocity(Vector3 direction)
    {
        if(mState != MoveState.dashing)
        {
            if(grounded)
            {
                float slopeMult = SlopeMultiplier(direction);
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, Vector3.up);
                if(horizontalVelocity.magnitude > maxVelocity * slopeMult)
                    rb.linearVelocity = Vector3.ProjectOnPlane(horizontalVelocity.normalized * maxVelocity * slopeMult, groundData.normal);
            }
            else
            {
                Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                if(horizontalVelocity.magnitude > airMaxVelocity)
                    rb.linearVelocity = new Vector3(horizontalVelocity.normalized.x, 0, horizontalVelocity.normalized.z) * airMaxVelocity + new Vector3(0, rb.linearVelocity.y, 0);
            }
        }
    }

    private void MovePlayer(Vector3 direction)
    {
        if(grounded)
        {
            Vector3 moveVector = Vector3.ProjectOnPlane(direction, groundData.normal).normalized;
            float slopeMult = SlopeMultiplier(direction);
            rb.AddForce(moveVector * acceleration * slopeMult, ForceMode.Force);
        }
        else
        {
            Vector3 moveVector = Vector3.ProjectOnPlane(direction, groundData.normal).normalized;
            rb.AddForce(moveVector * acceleration * airControl, ForceMode.Force);
        }
    }

    private void Jump()
    {
        float jumpForce = Mathf.Sqrt(2 * 19.62f * jumpHeight);

        rb.linearDamping = 0;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, (Vector3.up * jumpForce).y, rb.linearVelocity.z);
        mState = MoveState.jumping;
    }

    private void Timers()
    {
        if(dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
        if(dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void Dash(float horizontal, float vertical)
    {
        if(dashTimer <= 0 && dashCooldownTimer <= 0)
        {
            Vector3 direction;
            if(horizontal != 0 || vertical != 0)
            {
                direction = (orientation.forward * vertical + orientation.right * horizontal).normalized;
            }
            else
            {
                direction = orientation.forward.normalized;
            }
            rb.useGravity = false;
            rb.linearDamping = 0;
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(direction * dashForce, ForceMode.Impulse);

            dashTimer = dashTime;
            dashCooldownTimer = dashCooldownTime;
            mState = MoveState.dashing;
        }
    }
}
