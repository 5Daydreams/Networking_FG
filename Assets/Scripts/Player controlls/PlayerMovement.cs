using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float airDrag;
    private float baseMovementSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Crouching")]
    public float crouchSpeed;
    public float croucingYscale;
    private float standingYscale;
    private KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Sliding")]
    public float slideForce;
    public float maxSlideTime;
    public float slideYScale;
    private float slideTimer;
    private KeyCode slideKey = KeyCode.LeftControl;
    private bool isSliding;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask GroundMask;
    bool isGrounded;

    [Header("Steep Handeling")]
    public float maxSteepAngle;
    private RaycastHit steepHit;

    [Header("Player orientation")]
    public Transform bodyOrientation;

    [Header("Multiplayer")]
    public Alteruna.Avatar avatar;
    RigidbodySynchronizable rb;

    private KeyCode jumpKey = KeyCode.Space;

    float horizontalInput;
    float verticalInput;

    Rigidbody unityRb;

    Vector3 moveDirection;

    public MovementState movementState;
    public enum MovementState { running, crouching, air, sliding }


    [SerializeField]
    TextMeshProUGUI mText;

    private void Start()
    {
        if (!avatar.IsMe)
            return;
        standingYscale = transform.localScale.y;
        baseMovementSpeed = moveSpeed;
        rb = GetComponent<RigidbodySynchronizable>();
        unityRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!avatar.IsMe)
            return;
        mText.text = "Speed: " + (int)rb.velocity.magnitude;

        Inputs();
        SpeedControl();
        StateHandeler();

        GroundCheck();
    }

    private void FixedUpdate()
    {
        if (!avatar.IsMe)
            return;

        AdjustVelocity();
    }

    private void Inputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jump
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
            Jump();

        //Slide
        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput !=0))
            StartSlide();
        if (Input.GetKeyUp(slideKey) && isSliding)
            StopSlide();

        //Crouch
        if (Input.GetKeyDown(crouchKey))
            Crouch();
        if (Input.GetKeyUp(crouchKey))
            transform.localScale = new Vector3(transform.localScale.x, standingYscale, transform.localScale.z);
    }

    private void StateHandeler()
    {
        if (isSliding)
        {
            movementState = MovementState.sliding;
        }
        else if (Input.GetKey(crouchKey) && !isSliding)
        {
            Debug.Log("hej");
            movementState = MovementState.crouching;
            if (isGrounded)
                moveSpeed = crouchSpeed;
            else
                moveSpeed = baseMovementSpeed;
        }
        else if (isGrounded)
        {
            movementState = MovementState.running;
            moveSpeed = baseMovementSpeed;
        }
        else
        {
            movementState = MovementState.air;
            moveSpeed = baseMovementSpeed;
        }
    }

    private void AdjustVelocity()
    {
        moveDirection = bodyOrientation.forward * verticalInput + bodyOrientation.right * horizontalInput;
        unityRb.useGravity = !OnSteep();

        if (isSliding)
            SlidingMovement();

        if (OnSteep())
        {
            AddForce(GetSteepMoveDirection() * moveSpeed);
            AddDrag(groundDrag);

            if (rb.velocity.y > 0)
            {
                AddForce(Vector3.down * 9);
            }
        }
        else if (isGrounded)
        {
            AddForce(moveDirection.normalized * moveSpeed );
            //rb.velocity += moveDirection.normalized * moveSpeed * Time.deltaTime;
            AddDrag(groundDrag);
        }
        else if (!isGrounded)
        {
            AddForce(moveDirection.normalized * moveSpeed * airMultiplier);
            //rb.velocity += moveDirection.normalized * moveSpeed * airMultiplier * Time.deltaTime;
            AddDrag(airDrag);
        }
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSteep() && !readyToJump)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        readyToJump = false;
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        AddImpulse(new Vector3(0f, jumpForce, 0f));
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void Crouch()
    {
        transform.localScale = new Vector3(transform.localScale.x, croucingYscale, transform.localScale.z);
        if (readyToJump == true && isGrounded)
        {
            AddImpulse(Vector3.down * 5f);
            Debug.Log("Force down");
        }
    }

    private void StartSlide()
    {
        isSliding = true;
        transform.localScale = new Vector3(transform.localScale.x, slideYScale, transform.localScale.z);
        if (readyToJump == true && isGrounded)
        {
            AddImpulse(Vector3.down * 5f);
            Debug.Log("Force down");
        }
        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = bodyOrientation.forward * verticalInput + bodyOrientation.right * horizontalInput;
        AddForce(inputDirection.normalized * slideForce);

        slideTimer -= Time.deltaTime;
        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        isSliding = false;
        transform.localScale = new Vector3(transform.localScale.x, standingYscale, transform.localScale.z);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, GroundMask);
    }

    private bool OnSteep()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out steepHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, steepHit.normal);
            return angle < maxSteepAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSteepMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, steepHit.normal).normalized;
    }

    void AddForce(Vector3 Force)
    {
        rb.velocity += Force * Time.deltaTime;
    }

    void AddImpulse(Vector3 impulse)
    {
        rb.velocity += impulse;
    }

    void AddDrag(float drag)
    {
        rb.velocity *= (1 - Time.deltaTime * drag);
    }
}