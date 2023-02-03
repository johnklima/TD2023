using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed, runSpeed, bounceForce;
    private float currentSpeed;
    private Vector3 moveDir;
    
    private bool isRunning, isJumping, isMoving, isGrounded, onMushroomBounce;

    [SerializeField] private float jumpForce;
    private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask mushroomBounceLayer;

    private Vector3 velocity;

    private CharacterController controller;
    private Animator anim;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, .1f, groundLayer);
        onMushroomBounce = Physics.CheckSphere(groundCheck.position, .1f, mushroomBounceLayer);

        Move();
        Sprint();

        Jump();

        if (onMushroomBounce)
        {
            velocity.y = bounceForce;
        }
    }

    public bool IsMoving()
    {
        if (moveDir.magnitude <= 0f)
        {
            return false;
        }
        else
            return true;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public bool IsJumping()
    {
        return isGrounded;
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

        if (isGrounded && (velocity.y < 0f))
            velocity.y = 0f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }
}
