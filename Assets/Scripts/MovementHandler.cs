using System;
using Unity.VisualScripting;
using UnityEngine;
using static GameController;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private Transform orientation;

    [Header("Speed Controls")]
    [SerializeField] public float acceleration;
    [SerializeField] public float maxVelocity;

    [Header("Drag controls")]
    [SerializeField] private float groundDrag;
    [SerializeField] private LayerMask groundLayer;

    [Header("Ground")]
    public bool grounded;
    RaycastHit groundData;
    Vector3 groundNormal = Vector3.up;

    [Header("Jump controls")]
    [SerializeField] public float jumpHeight;
    [SerializeField] private float airControl;
    [SerializeField] private float airMaxVelocity;

    [Header("Dash controls")]
    [SerializeField] public float dashForce;
    [SerializeField] private float dashTime;
    [SerializeField] public float dashCooldownTime;
    private float dashTimer = 0;
    private float dashCooldownTimer = 0;

    public Rigidbody rb;

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
        GroundCast();

        Timers();
        StateMachine();
        Drag();
        LimitVelocity();

        if(gameController.GetGameState() != GameState.Normal)
        {
            direction = Vector3.zero;
            return;            
        }

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        direction = (orientation.forward * vertical + orientation.right * horizontal).normalized;

        if(Input.GetKeyDown("space") && grounded)
            Jump();

        if(Input.GetKeyDown("left shift"))
            StartDash(horizontal, vertical);
    }

    void FixedUpdate()
    {
        if(mState != MoveState.dashing)
            MovePlayer(direction);

        if(gameController.GetGameState() != GameState.DeathScreen)
            StickToGround();
    }

    private void StateMachine()
    {
        switch(mState)
        {
            case MoveState.walking:
                break;

            case MoveState.jumping:
            {
                Debug.Log($"{groundNormal}, {Vector3.Project(rb.linearVelocity, groundNormal)}");
                if(Vector3.Project(rb.linearVelocity, groundNormal).y <= .05f)
                {
                    mState = MoveState.walking;
                    Debug.Log($"Max height: {rb.gameObject.transform.position.y}");
                }
                break;
            }

            case MoveState.dashing:
            {
                Dash();
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
        grounded = Physics.SphereCast(orientation.position, .45f, Vector3.down, out groundData, .55f + .1f, groundLayer) && mState != MoveState.jumping;
        if(groundData.normal != Vector3.zero)
            groundNormal = groundData.normal;
        else
            groundNormal = Vector3.up;

        if(mState != MoveState.dashing)
            rb.useGravity = !grounded && Vector3.Angle(Vector3.up, groundNormal) <= 45;        
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

    private void LimitVelocity()
    {
        if(mState != MoveState.dashing)
        {
            if(grounded)
            {
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, groundNormal);
                if(horizontalVelocity.magnitude > maxVelocity)
                    rb.linearVelocity = Vector3.ProjectOnPlane(horizontalVelocity.normalized * maxVelocity, groundNormal);
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
            Vector3 moveVector = Vector3.ProjectOnPlane(direction, groundNormal).normalized;
            rb.AddForce(moveVector * acceleration, ForceMode.Force);
        }
        else
        {
            Vector3 moveVector = Vector3.ProjectOnPlane(direction, groundNormal).normalized;
            rb.AddForce(moveVector * acceleration * airControl, ForceMode.Force);
        }
    }

    void StickToGround()
    {
        if(grounded && mState != MoveState.jumping)
        {
            rb.AddForce(Vector3.Project(Physics.gravity, -groundNormal), ForceMode.Force);
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

    Vector2 dashInputs = Vector2.zero;
    private void StartDash(float horizontal, float vertical)
    {
        if(dashTimer <= 0 && dashCooldownTimer <= 0)
        {
            dashInputs = new Vector2(horizontal, vertical);
            rb.linearDamping = 0;
            dashTimer = dashTime;
            dashCooldownTimer = dashCooldownTime;
            mState = MoveState.dashing;
        }
    }

    private void Dash()
    {
        Vector3 direction;
        if(dashInputs.x != 0 || dashInputs.y != 0)
        {
            direction = (orientation.forward * dashInputs.y + orientation.right * dashInputs.x).normalized;
        }
        else
        {
            direction = orientation.forward.normalized;
        }
        
        Vector3 dash = Vector3.ProjectOnPlane(direction * dashForce, groundNormal);
        rb.linearVelocity = new Vector3(dash.x, 0, dash.z);
    }
}
