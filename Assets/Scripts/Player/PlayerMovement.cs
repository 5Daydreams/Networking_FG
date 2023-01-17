using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float groundDrag;
    private float moveSpeed;

    private float desiredMovespeed;
    private float lastDesiredMovespeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpSpeed;
    public float jumpCooldown;
    bool readyToJump = true;

    /*
    [Header("Crouching")]
    public float crouchSpeed;
    public float croucingYscale;
    private KeyCode crouchKey = KeyCode.LeftControl;
    */

    [Header("Sliding")]
    public float maxGroundSlideSpeed;
    public float maxSlopeSlideSpeed;
    public float slideForce;
    public float speedIncreaseMultiplier;
    public float steepIncreaseMultiplier;
    public float maxSlideTime;
    public float slideYScale;
    private float standingYscale;
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
    private bool exitingSteep;


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

    [HideInInspector]public MovementState movementState;
    public enum MovementState 
    { 
        running, 
        crouching,
        jumping,
        air, 
        sliding 
    }

    [Header("Debug")]
    [SerializeField]
    TextMeshProUGUI speedText;
    [SerializeField]
    TextMeshProUGUI stateText;


    private void Start()
    {
        if (!avatar.IsMe)
            return;
        standingYscale = transform.localScale.y;
        rb = GetComponent<RigidbodySynchronizable>();
        unityRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!avatar.IsMe)
            return;

        GroundCheck();

        Inputs();
        SpeedControl();
        StateHandeler();
    }

    private void FixedUpdate()
    {
        if (!avatar.IsMe)
            return;

        AdjustVelocity();

        speedText.text = "Speed: " + new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude.ToString("F1");
        stateText.text = movementState.ToString();

        if (isGrounded)
            AddDrag(groundDrag);
        else
            AddDrag(0);
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

        /*Crouch
        if (Input.GetKeyDown(crouchKey))
            Crouch();
        if (Input.GetKeyUp(crouchKey))
            transform.localScale = new Vector3(transform.localScale.x, standingYscale, transform.localScale.z);
        */
    }

    private void StateHandeler()
    {
        if (isSliding)
        {
            movementState = MovementState.sliding;

            if (OnSteep() && rb.velocity.y < 0.1f && isGrounded)
                desiredMovespeed = maxSlopeSlideSpeed;
            else if (isGrounded)
                desiredMovespeed = maxGroundSlideSpeed;
            else
                desiredMovespeed = walkSpeed;
        }
        else if (Input.GetKey(jumpKey))
        {
            movementState = MovementState.jumping;
            if (!readyToJump)
                desiredMovespeed = jumpSpeed;
        }
        /*else if (Input.GetKey(crouchKey) && !isSliding)
        {
            movementState = MovementState.crouching;
            if (isGrounded)
            {
                desiredMovespeed = crouchSpeed; 
                StopAllCoroutines();
                crouchSpeed = desiredMovespeed;
            }
            else
                desiredMovespeed = walkSpeed;
        }*/
        else if (isGrounded)
        {
            movementState = MovementState.running;
            desiredMovespeed = walkSpeed;
            if (horizontalInput == 0 && verticalInput == 0 )
            {
                StopAllCoroutines();
                moveSpeed = desiredMovespeed;
            }
        }
        else
        {
            movementState = MovementState.air;
        }

        //Set to base speed when we hit a wall
        if (Mathf.Approximately(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude, 0))
        {
            desiredMovespeed = walkSpeed;
            moveSpeed = desiredMovespeed;
            StopAllCoroutines();
        }
        else if (Mathf.Abs(desiredMovespeed - lastDesiredMovespeed) > 1f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(LerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMovespeed;
        }

        lastDesiredMovespeed = desiredMovespeed;
    }

    private IEnumerator LerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMovespeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            if (movementState == MovementState.running)
                moveSpeed = Mathf.Lerp(startValue, desiredMovespeed, time / (difference * 0.1f));
            else
                moveSpeed = Mathf.Lerp(startValue, desiredMovespeed, time / (difference));

            if (OnSteep())
            {
                float steepAngle = Vector3.Angle(Vector3.up, steepHit.normal);
                float steepAngleIncrease = 1 + (steepAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * steepIncreaseMultiplier * steepAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;


            yield return null;
        }

        moveSpeed = desiredMovespeed;
    }

    /*
    private IEnumerator LerpMoveSpeedTest()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMovespeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMovespeed, time / difference);
            time += Time.deltaTime;
            yield return null;
        }

        moveSpeed = desiredMovespeed;
    }
    */

    private void AdjustVelocity()
    {
        moveDirection = bodyOrientation.forward * verticalInput + bodyOrientation.right * horizontalInput;

        if (isSliding)
        {
            SlideMovement();
        }

        if (OnSteep() && readyToJump)
        {
            AddForce(GetSteepMoveDirection(moveDirection) * moveSpeed * 10f);

            if (rb.velocity.y > 0)
                AddForce(Vector3.down * 8);
        }
        else
            AddForce(moveDirection.normalized * moveSpeed * 10f);

        unityRb.useGravity = !OnSteep();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSteep() && readyToJump)
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
        AddImpulse(Vector3.up * jumpForce);
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    /*private void Crouch()
    {
        transform.localScale = new Vector3(transform.localScale.x, croucingYscale, transform.localScale.z);
        if (readyToJump && isGrounded)
            AddImpulse(Vector3.down * 5f);
    }*/

    private void StartSlide()
    {
        isSliding = true;
        transform.localScale = new Vector3(transform.localScale.x, slideYScale, transform.localScale.z);
        if (readyToJump && isGrounded)
            AddImpulse(Vector3.down * 5f);
      
        slideTimer = maxSlideTime;
    }

    private void SlideMovement()
    {
        Vector3 inputDirection = bodyOrientation.forward * verticalInput + bodyOrientation.right * horizontalInput;

        if (isGrounded)
        {
            if (!OnSteep() || rb.velocity.y > -0.1f)
            {
                AddForce(inputDirection.normalized * slideForce);
                slideTimer -= Time.deltaTime;
            }
            else
            {
                AddForce(GetSteepMoveDirection(inputDirection) * slideForce);
            }
            if (slideTimer <= 0)
                StopSlide();
        }
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

    private Vector3 GetSteepMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, steepHit.normal).normalized;
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