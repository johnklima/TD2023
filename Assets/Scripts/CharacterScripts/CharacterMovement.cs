using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed, runSpeed;
    private float currentSpeed;
    private Vector3 moveDir;
    
    private bool isRunning, isJumping, isMoving, isGrounded;

    [SerializeField] private float jumpForce;
    private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Vector3 yVelocity;

    private CharacterController controller;
    private Animator anim;

    private States playerStates = new States();
    private enum States
    {
        Idle,
        Walk,
        Run,
        InAir,
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, .1f, groundLayer);
        Move();
        isMoving = (moveDir.magnitude > 0) ? true : false;
        Sprint();

        Jump();


        if (!isGrounded)
        {
            playerStates = States.InAir;
        }
        else
        {
            if (!isMoving && !isRunning)
                playerStates = States.Idle;
            else
            {
                playerStates = (isRunning) ? States.Run : States.Walk;
            }
        }


    }

    

    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
            isRunning = true;
        } 
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = walkSpeed;
            isRunning = false;
        }
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        float verticalInput = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;

        moveDir = transform.forward * verticalInput + transform.right * horizontalInput;

        controller.Move(moveDir);

        if (isGrounded && (yVelocity.y < 0f))
        {
            yVelocity.y = 0f;
        }

        yVelocity.y += gravity * Time.deltaTime;
        controller.Move(yVelocity * Time.deltaTime);
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            yVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    private void AnimationStates()
    {
        switch (playerStates)
        {
            case States.Idle:

                anim.SetBool("IsRunning", true);
                anim.SetBool("IsWalking", true);
                anim.SetBool("IsJumping", true);

                break;

            case States.Walk:

                anim.SetBool("IsWalking", true);

                break;

            case States.Run:

                anim.SetBool("IsRunning", true);

                break;

                //assuming there will be one animation for the jumping animation that gets played once, otherwise the logic needs to change
            case States.InAir:

                anim.SetBool("IsRunning", false);
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsJumping", true);

                break;
        }
    }

}
