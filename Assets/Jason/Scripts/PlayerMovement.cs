using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float airDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Rotation")]
    public Transform bodyTransform;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask GroundMask;
    bool isGrounded;

    private KeyCode jumpKey = KeyCode.Space;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    [Header("Multiplayer")]
    public Alteruna.Avatar avatar;
    RigidbodySynchronizable rb;

    [SerializeField]
    TextMeshProUGUI mText;

    private void Start()
    {
        if (!avatar.IsMe)
            return;

        rb = GetComponent<RigidbodySynchronizable>();
    }

    private void Update()
    {
        if (!avatar.IsMe)
            return;
        mText.text = "Speed: " + (int)rb.velocity.magnitude;

        Inputs();
        SpeedControl();
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

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void AdjustVelocity()
    {
        moveDirection = bodyTransform.forward * verticalInput + bodyTransform.right * horizontalInput;

        if (isGrounded)
        {
            rb.velocity += moveDirection.normalized * moveSpeed * Time.deltaTime;
            AddDrag(groundDrag);
        }
        else if (!isGrounded)
        { 
            rb.velocity += moveDirection.normalized * moveSpeed * airMultiplier * Time.deltaTime;
            AddDrag(airDrag);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        // Limits player Velocity;
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        AddImpulse(new Vector3(0f, jumpForce, 0f));
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, GroundMask);
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