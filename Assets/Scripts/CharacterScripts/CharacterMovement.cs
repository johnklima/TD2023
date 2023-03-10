using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement Instance { get; private set; }


    [SerializeField] private float walkSpeed, runSpeed, bounceForce, rotationSpeed, rotationMultiplier, distanceToGround;
    private float currentSpeed;
    private Vector3 moveDir;
    
    private bool isRunning, isJumping, isMoving, isGrounded, onMushroomBounce, canMove = true, performedJump;

    [SerializeField] private float jumpForce, gravityMultiplier;
    private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask mushroomBounceLayer;

    [SerializeField] private Transform playerBody;
    private Vector3 velocity;

    private CharacterController controller;
    private Animator anim;

    ////////////// <movement sound> /////////////////////

    // from here down we can place all code related to character sound
    // and it is in one nice tight place. perhaps it is a class of its
    // own, but we will prolly want to hook into private stuff inside
    // the movement class, without having to expose those properties.
    // Add any other new character variables ABOVE this block.

    FMOD.Studio.EventInstance ewalk;

    public float isOnPebbles = 0;
    public float isOnForest = 0;
    public float isOnDirt = 0;

    [SerializeField] private float walkThresh = 0.02f;

    private Vector3 prevPos;


    private void doSound()
    {
        if (Vector3.Distance(transform.position, prevPos) > walkThresh)
        {

            prevPos = transform.position;


            ewalk.setPaused(false);

            /* //walking is 2d, no need for this. use this if it is a 3d sound
            FMOD.ATTRIBUTES_3D structure;
            FMOD.VECTOR pos;
            ewalk.get3DAttributes(out structure);
            pos.x = transform.position.x;
            pos.y = transform.position.y;
            pos.z = transform.position.z;
            structure.position = pos;
            ewalk.set3DAttributes(structure);           
            */

            ewalk.setParameterByName("Dirt", isOnDirt);
            ewalk.setParameterByName("Pebbles", isOnPebbles);
            ewalk.setParameterByName("Forest", isOnForest);



        }
        else
        {

            ewalk.setPaused(true);
        }

    }

    ////////////// </movement sound> /////////////////////

    private void Awake()
    {
        Instance = this;


        ewalk = FMODUnity.RuntimeManager.CreateInstance("event:/V2 Sound (Coders)/Footsteps_V2");
        ewalk.start();
        ewalk.setPaused(true);
    }




    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, distanceToGround, groundLayer);
        onMushroomBounce = Physics.CheckSphere(groundCheck.position, .1f, mushroomBounceLayer);

        Move();
        Sprint();

        Jump();

        doSound();

        if (onMushroomBounce)
        {
            velocity.y = bounceForce;
        }

        if (Input.GetMouseButtonDown(1) && isGrounded)
        {
            canMove = false;
            PlayerAnimations.Instance.PlayAimAnim(true);
        } else if (Input.GetMouseButtonUp(1))
        {
            canMove = true;
            PlayerAnimations.Instance.PlayAimAnim(false);
        }

        //Debug.Log(isGrounded);
    }

    public bool CanMove()
    {
        return canMove;
    }

    public bool PerformedJump()
    {
        return performedJump;
    }

    public bool IsMoving(){
        if (moveDir.magnitude <= 0f)
        {
            return false;
        }
        else
            return true;
    }

    public bool IsRunning(){
        return isRunning;
    }

    public bool IsJumping(){
        return isJumping;
    }

    public bool IsGrounded()
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
        if (!canMove) return;
        float horizontalInput = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        float verticalInput = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;

        moveDir = transform.forward * verticalInput + transform.right * horizontalInput;

        controller.Move(moveDir);

        if (isGrounded && (velocity.y < 0f))
            velocity.y = 0f;

        velocity.y += gravity * Time.deltaTime * gravityMultiplier;
        controller.Move(velocity * Time.deltaTime);

        //playerBody.forward += Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 20f);

        ////rotate the player based on the inputs
        //Quaternion toRotation = quaternion.LookRotation(playerBody.forward, Vector3.up);
        //playerBody.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        if (moveDir != Vector3.zero)
        {
            Quaternion toRotation = quaternion.LookRotation(moveDir, Vector3.up);
            playerBody.rotation = Quaternion.RotateTowards(playerBody.rotation, toRotation, rotationSpeed * rotationMultiplier * Time.deltaTime);
        }

    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            performedJump = true;
            PlayerAnimations.Instance.InitLanding();
            Invoke("ResetJump", 1f);
        }
    }

    private void ResetJump()
    {
        performedJump = false;
    }
}
