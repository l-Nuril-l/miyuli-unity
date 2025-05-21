using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.UI;
using System;

public class PlayerMovement : NetworkBehaviour
{
    public float movementForce;
    public float fallMultiplier;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float crouchMultiplier;
    bool readyToJump;
    long lastJump;
    long lastJumpStart;

    public float walkSpeed;
    public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool isGrounded;
    bool isRunning;

    float horizontalInput;
    float verticalInput;

    public bool isCrouching;
    public Transform cameraPos;

    Vector3 moveDirection;

    Rigidbody rb;

    private Animator animator;
    public Joystick joystick;

    private States State
    {
        get => (States)animator.GetInteger("state");
        set => animator.SetInteger("state", (int)value);
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        State = States.Idle;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        joystick = FindFirstObjectByType<Joystick>();

        if (isLocalPlayer && PlatformChecker.IsMobilePlatform())
        {
            GameObject.FindWithTag("uiJump").GetComponent<Button>().onClick.AddListener(Jump);
            GameObject.FindWithTag("uiCrouch").GetComponent<Button>().onClick.AddListener(SwitchCrouching);
        }
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        // ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (isGrounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;

        if (!isGrounded) rb.linearVelocity += Vector3.down * fallMultiplier * Time.deltaTime;
        //if (!isGrounded && rb.linearVelocity.y < 0)
        //{
        //    Debug.Log("B mmm" + rb.linearVelocity);
        //    Debug.Log("D" + Time.deltaTime);
        //    Debug.Log("n d" + Vector3.down * fallMultiplier);
        //    Debug.Log("w d" + Vector3.down * fallMultiplier * Time.deltaTime);
        //    Debug.Log("A mmm" + rb.linearVelocity);
        //}
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        if (PlatformChecker.IsMobilePlatform())
        {
            horizontalInput = joystick.Horizontal;
            verticalInput = joystick.Vertical;

            float magnitude = new Vector2(horizontalInput, verticalInput).magnitude;
            float threshold = 0.8f;

            if (magnitude >= threshold)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
        }
        else
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        // when to jump
        if (Input.GetKeyDown(jumpKey))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SwitchCrouching(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            SwitchCrouching(false);
        }
    }

    public void SwitchCrouching()
    {
        SwitchCrouching(!isCrouching);
    }

    public void SwitchCrouching(bool newCrouching)
    {
        if (newCrouching)
        {
            isCrouching = true;
            cameraPos.position -= Vector3.up;
        }
        else
        {
            isCrouching = false;
            cameraPos.position += Vector3.up;
        }
    }

    private bool prevIsGrounded;

    private void MovePlayer()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // calculate movement direction
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        Vector3 force = moveDirection.normalized * movementForce;
        // sprint
        force *= (isRunning && isGrounded && !isCrouching) ? sprintSpeed : walkSpeed;
        // on ground
        force *= isGrounded ? 1 * (isCrouching ? crouchMultiplier : 1) : airMultiplier;

        // apply force
        rb.AddForce(force, ForceMode.Force);

        //Debug.Log("Is Grounded" + isGrounded);
        //Debug.Log("Rdy to Jump" + readyToJump);

        if (readyToJump)
        {
            if (horizontalInput > 0 || verticalInput > 0)
            {
                State = Input.GetKey(KeyCode.LeftControl) ? States.StealthWalk : isRunning ? States.Run : States.Walk;
            }
            else
            {
                State = Input.GetKey(KeyCode.LeftControl) ? States.StealthIdle : States.Idle;
            }
        }
        else
        {
            double secondsSinceLastJumpStart = (DateTime.Now.Ticks - lastJumpStart) / (double)TimeSpan.TicksPerSecond;
            //Debug.Log(rb.linearVelocity);
            Debug.Log(State);
            if (State == States.JumpStart && (!isGrounded || secondsSinceLastJumpStart > 0.1))
            {
                State = States.JumpLand;
            }
            //if (rb.linearVelocity.y <= 0)
            {
                if (rb.linearVelocity.y < -10)
                {
                    State = States.FreeFall;
                }
                else if (rb.linearVelocity.y < 0 || !isGrounded)
                {
                    State = States.JumpLand;
                }
                //rb.linearVelocity.y <= 0 && 
                if (isGrounded && State != States.JumpStart)
                {
                    ResetJump();
                }
            }
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if ((flatVel.magnitude > walkSpeed))
        {
            Vector3 limitedVel = flatVel.normalized * (isRunning ? sprintSpeed : walkSpeed);
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    public void Jump()
    {
        double secondsSinceLastJump = (DateTime.Now.Ticks - lastJump) / (double)TimeSpan.TicksPerSecond;

        if (!readyToJump || !isGrounded || secondsSinceLastJump < jumpCooldown) return;


        State = States.JumpStart;
        readyToJump = false;
        lastJumpStart = DateTime.Now.Ticks;

        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
        lastJump = DateTime.Now.Ticks;
    }
}