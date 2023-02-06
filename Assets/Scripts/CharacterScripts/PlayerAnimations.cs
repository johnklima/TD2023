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
        

        animator.SetBool("IsWalking", characterMovement.IsMoving()); //if IsMoving() then set is moving bool to true
        animator.SetBool("IsRunning", characterMovement.IsRunning()); // if IsRunning() then set is running bool to true
        animator.SetBool("IsJumping", characterMovement.IsJumping());

        animator.SetBool("InAir", !characterMovement.IsGrounded());
    }
}
