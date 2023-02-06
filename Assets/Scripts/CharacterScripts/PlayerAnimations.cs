using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{

    private Animator animator;
    [SerializeField] private CharacterMovement characterMovement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (characterMovement.IsGrounded())
        {
            animator.SetBool("IsWalking", characterMovement.IsMoving());
            animator.SetBool("IsRunning", characterMovement.IsRunning());
            animator.SetBool("IsJumping", !characterMovement.IsJumping());
        }
        else
            animator.SetBool("InAir", !characterMovement.IsGrounded());


    }
}
